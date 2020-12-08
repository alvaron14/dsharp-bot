using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Bot_Discord_CSharp.Commands
{
    class TestCommand : BaseCommandModule
    {
        [Command("ping")]
        [Description("Dices ping y te responde pong")]
        [RequireRoles(RoleCheckMode.Any, "Tester", "Senador")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("response")]
        public async Task Response(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);

            await ctx.Member.SendMessageAsync("Hijo de puta");
        }

        [Command("respondReaction")]
        public async Task ResponseReaction(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }

        [Command("acontecimientos")]
        [Description("Muestra todos los acontecimientos históricos que han pasado hoy")]
        public async Task GetDocumentWeb(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            Uri address = new Uri("https://es.wikipedia.org/wiki/" + Today().Replace(" ", "_"));
            HtmlNode node = GetHtmlNodeFromPage(address);

            //< innertext >
            int numH2 = 0;
            List<ulong> emojiKeyList = ctx.Guild.Emojis.Keys.Cast<ulong>().ToList();
            string extractedData = "";

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
            List<DiscordEmbed> embedList = new List<DiscordEmbed>();
            embedBuilder.WithTitle("Acontecimientos del día: " + Today());

            foreach (HtmlNode line in node.Descendants())
            {
                if (line.Name.ToString() == "h2" && line.OuterHtml.Contains("mw-headline")) numH2++;
                if (numH2 == 1 && line.Name == "li")
                {
                    if (extractedData.Length + line.InnerText.Length > 2000)
                    {
                        embedList.Add(embedBuilder.WithDescription(extractedData).Build());
                        extractedData = "";
                    }
                    ulong randomEmoji = emojiKeyList[new Random().Next(0, emojiKeyList.Count)];
                    extractedData += DiscordEmoji.FromGuildEmote(ctx.Client, randomEmoji) + line.InnerText + "\n";
                }
                else if (numH2 > 1) break;
            }
            embedList.Add(embedBuilder.WithDescription(extractedData).Build());
            await SendEmbeds(ctx, embedList);
        }

        [Command("fecha")]
        public async Task GetCurrentDay(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync(Today());
        }

        [Command("borrar")]
        public async Task DeleteAllMessages(CommandContext ctx)
        {
            IReadOnlyCollection<DiscordMessage> messages = await ctx.Channel.GetMessagesAsync(100);
            await ctx.Channel.DeleteMessagesAsync(messages);
        }

        [Command("random")]
        [Description("Te da un número aleatorio entre 0 y X o entre X y Y")]
        public async Task GetRandomNumber(CommandContext ctx, params string[] command)
        {
            if (!command.Any())
            {
                await ctx.Channel.SendMessageAsync("Se necesita uno o dos valores para funcionar." +
                                                    "\nEj: `!random 5` o `!random 5 10`");
            } else
            {
                int length = command.Length;
                await ctx.Channel.SendMessageAsync(new Random().Next(length > 1 ? Int32.Parse(command[0]) : 0,
                                                                 Int32.Parse(command[length - 1]) + 1).ToString());
            }  
        }

        [Command("dice")]
        [Description("Tira un dado de 6 caras")]
        public async Task ThrowDice(CommandContext ctx)
        {
                await ctx.Channel.SendMessageAsync((new Random().Next(6) + 1).ToString());
        }

        [Command("coin")]
        [Description("Tira una moneda")]
        public async Task ThrowCoin(CommandContext ctx)
        {
            int randomValue = new Random().Next(2);
            string ladoMoneda = randomValue == 0 ? "Cara" : "Cruz";
            await ctx.Channel.SendMessageAsync(ladoMoneda);
        }

        private string Today()
        {                                                                       
            DateTime currentTime = DateTime.Today;
            IFormatProvider culture = new CultureInfo("es-ES", true);
            string[] currentTimeToString = currentTime.GetDateTimeFormats('m', culture);
            return currentTimeToString[0];
        }

        private HtmlNode GetHtmlNodeFromPage(Uri url)
        {
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

            string sDocument = webClient.DownloadString(url);

            var document = new HtmlDocument();
            document.LoadHtml(sDocument);

            HtmlNode node = document.DocumentNode.SelectSingleNode("//div[@class=\"mw-parser-output\"]");

            string output = node != null ? node.InnerText : "Error!!";
            System.Diagnostics.Debug.WriteLine(output);

            return node;
        }

        private async Task SendEmbeds(CommandContext ctx, List<DiscordEmbed> embedList)
        {
            List<string> colours = ProgresiveColour(embedList.Count());
            for (int i = 0; i < embedList.Count; i++)
            {
                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder(embedList[i]);
                embedBuilder.WithTitle(embedBuilder.Title + " | Parte " + (i+1) + "/" + embedList.Count());
                embedBuilder.WithColor(new DiscordColor(colours[i]));
                await ctx.Channel.SendMessageAsync("", false, embedBuilder.Build());
            }
        }

        private List<string> ProgresiveColour(int steps)
        {
            List<string> colours = new List<string>();
            for (int i = 1; i <= steps; i++)
            {
                int decimalHex = ColourCalculus(i, steps);
                string hex = decimalHex.ToString("x");
                colours.Add(FormatHex(hex));
            }
            return colours;
        }

        private int ColourCalculus(int iteration, int max)
        {
            if (iteration == 1)
            {
                return (int)((15 * Math.Pow(16, 5)) + (15 * Math.Pow(16, 4)));
            }
            if (iteration != 1 && iteration != max)
            {
                return (int)((15 / (iteration - 1) * Math.Pow(16, 5)) + (15 / (iteration - 1) * Math.Pow(16, 4))
                    + (15 / (max - iteration) * Math.Pow(16, 3)) + (15 / (max - iteration) * Math.Pow(16, 2)));
            }
            return (int)((15 * Math.Pow(16, 3)) + (15 * Math.Pow(16, 2)));
        }

        private string FormatHex(string hex)
        {
            if (hex.Length == 4) return "#00" + hex;
            return "#" + hex;
        }
    }
}

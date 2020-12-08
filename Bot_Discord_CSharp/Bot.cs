using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bot_Discord_CSharp.Dto;
using Bot_Discord_CSharp.Commands;
using Microsoft.Extensions.Logging;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;

namespace Bot_Discord_CSharp
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            string token, prefix;
            if (!Environment.GetEnvironmentVariables().Contains("TOKEN"))
            {
                var json = string.Empty;

                using (var fs = File.OpenRead(@"E:\Proyectos Visual Studio\Bot_Discord_CSharp\Bot_Discord_CSharp\config.json"))
                using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);

                var configJson = JsonConvert.DeserializeObject<ConfigDto>(json);
                token = configJson.Token;
                prefix = configJson.Prefix;
            }
            else
            {
                token = Environment.GetEnvironmentVariable("TOKEN");
                prefix = Environment.GetEnvironmentVariable("PREFIX");
            }

            var config = new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(1)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            RegisterCommands();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private async Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            DiscordGuild guild = await client.GetGuildAsync(732743391548407888, true);
            DiscordChannel channel = guild.GetChannel(785719482924007424);
            await channel.SendMessageAsync("Ya estoy de vuelta");
        }

        private void RegisterCommands()
        {
            Commands.RegisterCommands<TestCommand>();
        }
    }
}

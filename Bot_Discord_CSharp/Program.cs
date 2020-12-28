namespace Bot_Discord_CSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}

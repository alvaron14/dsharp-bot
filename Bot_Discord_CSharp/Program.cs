using ikvm.runtime;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Bot_Discord_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).
               UseKestrel().
               UseUrls("http://0.0.0.0:" + Environment.GetEnvironmentVariable("$PORT")).
               Build();

            host.Run();

            Bot bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

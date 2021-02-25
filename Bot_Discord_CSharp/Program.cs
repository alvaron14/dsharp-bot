using log4net;
using System;

namespace Bot_Discord_CSharp
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain currentDomain = default(AppDomain);
            currentDomain = AppDomain.CurrentDomain;
            // Handler for unhandled exceptions.
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;

            Bot bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;
            ILog log = LogManager.GetLogger(typeof(Program));
            log4net.Config.XmlConfigurator.Configure();

            log.Error(ex.Message + "\n" + ex.StackTrace);
        }
    }
}

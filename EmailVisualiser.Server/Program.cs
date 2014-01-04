using System;
using System.Diagnostics;
using System.IO;

using Nancy.Hosting.Self;

namespace EmailVisualiser.Server
{
    class Program
    {
        public static IndentingConsole Output = new IndentingConsole();
        private const string startUrl = "http://localhost:62259";

        [STAThread]
        static void Main(string[] args)
        {
            var host = new NancyHost(new Uri(startUrl));

            Console.WriteLine("Launching Email Visualiser.");
            try
            {
                host.Start();
                Console.WriteLine("Server has launched.");
                Console.WriteLine("Launching browser...");
                Process.Start(startUrl);
                Console.WriteLine("Press any key to stop the server.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                TextWriter errorWriter = Console.Error; //TODO change to a logger
                errorWriter.WriteLine("------An error has been encountered.------");
                errorWriter.WriteLine(e.Message);
#if DEBUG
                errorWriter.WriteLine(e.InnerException);
                errorWriter.WriteLine(e.StackTrace);
#endif
                errorWriter.WriteLine("------The application will now close.------");
            }
            finally
            {
                host.Stop();
            }
        }
    }
}

using System;
using System.Diagnostics;

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
            using (var host = new NancyHost(new Uri(startUrl)))
            {
                Console.WriteLine("Launching Email Visualiser.");
                host.Start();
                Console.WriteLine("Server has launched.");
                Console.WriteLine("Launching browser...");
                Process.Start(startUrl);
                Console.ReadLine();
            }
        }
    }
}

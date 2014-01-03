using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Controllers;
using EmailVisualiser.Models;
using EmailVisualiser.ConsoleApp.Views;
using EmailVisualiser.Analysis;
using EmailVisualiser.Data;

namespace EmailVisualiser.ConsoleApp
{
    public class EntryPoint
    {
        public static IndentingConsole Output = new IndentingConsole();

        [STAThread]
        public static void Main()
        {
            DataStorage data = new DataStorage();
            DataAnalysisEngine analysisEngine = new DataAnalysisEngine(data);
            MainMenuModel mainMenuModel = new MainMenuModel(data, analysisEngine);
            using (var mainMenu = new MainMenuView(mainMenuModel))
            {
                try
                {
                    mainMenu.Run();
                }
                catch(Exception e)
                {
                    TextWriter errorWriter = Console.Error;
                    errorWriter.WriteLine("------An error has been encountered.------");
                    errorWriter.WriteLine(e.Message);
#if DEBUG
                    errorWriter.WriteLine(e.InnerException);
                    errorWriter.WriteLine(e.StackTrace);
#endif
                    errorWriter.WriteLine("------The application will now close.------");

                }
            }

            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParserConsoleApp.Controllers;
using OutlookParserConsoleApp.Models;
using OutlookParserConsoleApp.Views;

namespace OutlookParserConsoleApp
{
    public class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            Console.WriteLine("Welcome to the Outlook Parser application.");
            MainMenuModel mainMenuModel = new MainMenuModel();
            MainMenuView mainMenu = new MainMenuView(mainMenuModel);
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

            Console.ReadLine();
        }
    }
}

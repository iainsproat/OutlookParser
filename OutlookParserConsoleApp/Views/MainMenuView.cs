using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParser;
using EmailVisualiser.Views;
using EmailVisualiser.Models;
using EmailVisualiser.Controllers;

namespace EmailVisualiser.ConsoleApp.Views
{
    public class MainMenuView : IView<MainMenuController, MainMenuModel>, IDisposable
    {
        private bool _disposed;
        protected MainMenuModel mdl;
        protected MainMenuController contrlr;

        public MainMenuView(MainMenuModel model)
        {
            this._disposed = false;
            this.mdl = model;
            this.Register(model);
        }

        #region MVC boilerplate
        public MainMenuModel Model
        {
            get
            {
                return this.mdl;
            }
        }

        public MainMenuController Controller
        {
            get
            {
                if (this.contrlr == null)
                {
                    this.contrlr = new MainMenuController(this.Model);
                }

                return this.contrlr;
            }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Controller cannot be null.");
                }

                if (this.contrlr != null)
                {
                    throw new InvalidOperationException("A controller for this view has already been set.");
                }

                this.contrlr = value;
            }
        }

        public void Register(MainMenuModel model)
        {
            // empty
        }

        public void Release(MainMenuModel model)
        {
            // empty
        }

        #region IDisposal
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.Model != null)
                {
                    this.Release(this.Model);
                }
            }

            //free resources
            this.mdl = null;
            this.contrlr = null;

            this._disposed = true;
        }
        #endregion

        #endregion

        public void Run()
        {
            Console.WriteLine("Welcome to the email analyser and visualiser");
            Console.WriteLine("This application currently only reads Pst files.");
            EntryPoint.Output.Indent++;
            while (true)
            {
                this.DisplayMainMenuWelcome();
                this.DisplayMainMenuOptions();
                if(!this.RespondToMainMenuOptions())
                {
                    break; // the user wishes to exit.
                }
            }

            EntryPoint.Output.Indent--;
            Console.WriteLine("Exiting application.");
        }

        public void DisplayMainMenuWelcome()
        {
            Console.WriteLine("---Main Menu---");
            Console.WriteLine("{0} emails already exist in the database.", this.Model.NumberOfExistingEmails());
        }

        public void DisplayMainMenuOptions()
        {
            Console.WriteLine("Press one of the following keys to choose from the below options:");
            Console.WriteLine("1 = Add additional data.");
            Console.WriteLine("2 = Work with the existing data.");
            Console.WriteLine("3 = Delete all data.");
            Console.WriteLine("e = Exit the application.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>False if the user wishes to exit, otherwise true</returns>
        public bool RespondToMainMenuOptions()
        {
            string userInput = Console.ReadLine();

            EntryPoint.Output.Indent++;
            switch(userInput.ToLowerInvariant())
            {
                case "1":
                    this.SpawnAddAdditionalDataDialog();
                    break;
                case "2":
                    this.SpawnDataVisualisationView();
                    break;
                case "3":
                    this.DeleteAllEmailsDialog();
                    break;
                case "e":
                case "exit":
                    return false; //user wishes to exit
                default:
                    Console.WriteLine("The option was not recognised, please try again!");
                    break;
            }

            EntryPoint.Output.Indent--;

            return true;
        }

        private void SpawnAddAdditionalDataDialog()
        {
            var dialogModel = new AddAdditionalDataModel(this.Model.Data);
            using (var dialogView = new AddAdditionalDataView(dialogModel))
            {
                dialogView.Show();
            }
        }

        private void SpawnDataVisualisationView()
        {
            var spawnedModel = new DataVisualisationModel(this.Model.AnalysisEngine);
            using (var spawnedView = new DataVisualisationView(spawnedModel))
            {
                spawnedView.Show();
            }
        }

        private void DeleteAllEmailsDialog()
        {
            Console.WriteLine("Do you really wish to delete the existing data? Press 'y' to confirm or press any other key to return to the main menu.");
            string confirmation = Console.ReadLine();
            if (confirmation.ToLowerInvariant() == "y")
            {
                Console.WriteLine("Deleting all emails.");
                this.Controller.DeleteAllEmails();
                Console.WriteLine("All emails are now deleted.");
            }
            else
            {
                Console.WriteLine("You chose not to delete all the emails, and we will return to the main menu.");
            }
        }

        public static bool IsUserTryingToExit(string userInput) //HACK - used by spawned dialogs
        {
            switch(userInput.ToLowerInvariant())
            {
                case "e":
                case "exit":
                    return true;
            }

            return false;
        }
    }
}

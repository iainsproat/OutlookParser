using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParserConsoleApp.Models;
using OutlookParserConsoleApp.Controllers;

namespace OutlookParserConsoleApp.Views
{
    class DataVisualisationView : IView<DataVisualisationController, DataVisualisationModel>, IDisposable
    {
        private bool _disposed;
        protected DataVisualisationModel mdl;
        protected DataVisualisationController contrlr;

        public DataVisualisationView(DataVisualisationModel model)
        {
            this._disposed = false;
            this.mdl = model;
            this.Register(model);
        }

        #region MVC boilerplate
        public DataVisualisationModel Model
        {
            get
            {
                return this.mdl;
            }
        }

        public DataVisualisationController Controller
        {
            get
            {
                if (this.contrlr == null)
                {
                    this.contrlr = new DataVisualisationController(this.Model);
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

        public void Register(DataVisualisationModel model)
        {
            this.Model.EmailsGroupedCountedAndSortedByDate += this.DisplaySortedEmailsByDate;
        }

        public void Release(DataVisualisationModel model)
        {
            this.Model.EmailsGroupedCountedAndSortedByDate -= this.DisplaySortedEmailsByDate;
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

        public void Show()
        {
            while (true)
            {
                this.DisplayMenuOptions();
                if(!this.RespondToUser())
                {
                    break; //user wishes to exit
                }
            }
        }

        private void DisplayMenuOptions()
        {
            Console.WriteLine("From the below options, please choose what visualisation to view:");
            Console.WriteLine("1 - Count of emails received each day.");
            Console.WriteLine("e - Exit");
        }

        private bool RespondToUser()
        {
            string userInput = Console.ReadLine();
            switch (userInput.ToLowerInvariant())
            {
                case "1":
                    Console.WriteLine("Sorting emails by date.");
                    this.Controller.GetEmailDailyCountSortedByDate();
                    break;
                case "e":
                case "exit":
                    return false;
                default:
                    Console.WriteLine("Option is not understood, please try again.");
                    break;
            }

            return true;
        }

        protected void DisplaySortedEmailsByDate(IEnumerable<Tuple<DateTime, int>> results)
        {
            foreach (var line in results)
            {
                Console.WriteLine("{0} {1}", line.Item1, line.Item2);
            }
        }
    }
}

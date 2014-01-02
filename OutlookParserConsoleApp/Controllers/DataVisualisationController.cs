using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParserConsoleApp.Models;

namespace OutlookParserConsoleApp.Controllers
{
    class DataVisualisationController : IController<DataVisualisationModel>
    {
        private readonly DataVisualisationModel mdl;

        public DataVisualisationController(DataVisualisationModel menuModel)
        {
            this.mdl = menuModel;
        }

        public DataVisualisationModel Model
        {
            get
            {
                return this.mdl;
            }
        }

        public IEnumerable<Tuple<DateTime, int>> GetEmailDailyCountSortedByDate()
        {
            return this.Model.GetEmailDailyCountSortedByDate();
        }
    }
}

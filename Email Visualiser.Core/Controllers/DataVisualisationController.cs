using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Models;

namespace EmailVisualiser.Controllers
{
    public class DataVisualisationController : IController<DataVisualisationModel>
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

        public void GetEmailDailyCountSortedByDate()
        {
            this.Model.GetEmailDailyCountSortedByDate();
        }
    }
}

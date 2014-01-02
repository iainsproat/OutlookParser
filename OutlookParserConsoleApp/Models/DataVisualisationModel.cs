using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParserConsoleApp.Services;

namespace OutlookParserConsoleApp.Models
{
    class DataVisualisationModel : IModel
    {
        private readonly DataAnalysisEngine _analysis;

        public DataVisualisationModel(DataAnalysisEngine analysisEngine)
        {
            if(analysisEngine == null)
            {
                throw new ArgumentNullException("analysisEngine");
            }

            this._analysis = analysisEngine;
        }

        public IEnumerable<Tuple<DateTime, int>> GetEmailDailyCountSortedByDate()
        {
            return this._analysis.GetEmailDailyCountSortedByDate();
        }
    }
}

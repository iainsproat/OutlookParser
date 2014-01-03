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
        public event Action<IEnumerable<Tuple<DateTime, int>>> EmailsGroupedCountedAndSortedByDate;

        protected void RaiseEmailsGroupedCountedAndSortedByDate(IEnumerable<Tuple<DateTime, int>> results)
        {
            if (EmailsGroupedCountedAndSortedByDate != null)
            {
                EmailsGroupedCountedAndSortedByDate(results);
            }
        }

        public DataVisualisationModel(DataAnalysisEngine analysisEngine)
        {
            if(analysisEngine == null)
            {
                throw new ArgumentNullException("analysisEngine");
            }

            this._analysis = analysisEngine;
        }

        public void GetEmailDailyCountSortedByDate()
        {
            var results = this._analysis.GetEmailDailyCountSortedByDate();
            this.RaiseEmailsGroupedCountedAndSortedByDate(results);
        }
    }
}

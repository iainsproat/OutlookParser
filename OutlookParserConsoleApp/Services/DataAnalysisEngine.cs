using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookParserConsoleApp.Services
{
    public class DataAnalysisEngine
    {
        private readonly DataStorage _data;

        public DataAnalysisEngine(DataStorage dataStorage)
        {
            if (dataStorage == null)
            {
                throw new ArgumentNullException("dataStorage");
            }

            this._data = dataStorage;
        }

        public DataStorage Data
        {
            get
            {
                return this._data;
            }
        }

        public IEnumerable<Tuple<DateTime, int>> GetEmailDailyCountSortedByDate()
        {
            return this.Data.AllEmails.GroupBy(email => email.ReceivedTime.Date)
                        .Select(group => new
                        Tuple<DateTime, int>(group.Key,
                            group.Count()))
                        .OrderBy(x => x.Item1);
        }
    }
}

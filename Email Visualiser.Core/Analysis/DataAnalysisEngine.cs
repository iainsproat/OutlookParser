using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Data;
using EmailVisualiser.Analysis.Graph;

namespace EmailVisualiser.Analysis
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

        public IGraph<string> WeightedGraphOfSendersAndRecipients()
        {

                var graph = new WeightedGraph<string>();
                foreach (var email in this.Data.AllEmails)
                {
                    if (string.IsNullOrWhiteSpace(email.Sender))
                    {
                        continue;
                    }

                    foreach (var recipient in email.Recipients)
                    {
                        if (string.IsNullOrWhiteSpace(recipient))
                        {
                            continue;
                        }

                        graph.Connect(email.Sender, recipient);
                    }
                }

                return graph;
        }
    }
}

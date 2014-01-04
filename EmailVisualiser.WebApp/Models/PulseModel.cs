using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailVisualiser.Data;

namespace EmailVisualiser.WebApp.Models
{
    public class PulseModel
    {
        private readonly DataStorage _data;
        public PulseModel(DataStorage dataStore)
        {
            this._data = dataStore;
        }

        public string EarliestDate
        {
            get
            {
                var dateTime = this._data.AllEmails.Min(e =>
                    {
                        return e.ReceivedTime;
                    });
                return dateTime.ToShortDateString();
            }
        }

        public string LatestDate
        {
            get
            {
                var dateTime = this._data.AllEmails.Max(e =>
                    {
                        return e.ReceivedTime;
                    });
                return dateTime.ToShortDateString();
            }
        }

        public int TotalNumberOfEmails
        {
            get
            {
                return this._data.Count;
            }
        }

        public int NumberOfInternalSenders
        {
            get
            {
                return -1;
            }
        }

        public int NumberOfOutgoingEmails
        {
            get
            {
                return -1;
            }
        }

        public int NumberOfExternalRecipients
        {
            get
            {
                return -1;
            }
        }

        public int NumberOfIncomingEmails
        {
            get
            {
                return -1;
            }
        }

        public int NumberOfExternalSenders
        {
            get
            {
                return -1;
            }
        }

        public int NumberOfInternalEmails
        {
            get
            {
                return -1;
            }
        }

        public int NumberOfOutgoingAttachments
        {
            get
            {
                return -1;
            }
        }

        public int NumberOfIncomingAttachments
        {
            get
            {
                return -1;
            }
        }
    }
}

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
                return this._data.TotalNumberOfEmails;
            }
        }

        public int NumberOfInternalSenders
        {
            get
            {
                //unique addresses who have sent an outgoing email
                return -1;
            }
        }

        public int NumberOfOutgoingEmails
        {
            get
            {
                //sender is Ramboll
                //at least one recipient is not Ramboll
                return -1;
            }
        }

        public int NumberOfExternalRecipients
        {
            get
            {
                //unique addresses who are recipients to an outgoing email
                return -1;
            }
        }

        public int NumberOfIncomingEmails
        {
            get
            {
                //sender is not Ramboll
                return -1;
            }
        }

        public int NumberOfExternalSenders
        {
            get
            {
                //unique addresses of incoming emails
                return -1;
            }
        }

        public int NumberOfInternalEmails
        {
            get
            {
                //sender is Ramboll
                //all recipients are Ramboll
                return -1;
            }
        }

        public int NumberOfOutgoingAttachments
        {
            get
            {
                //attachment to outgoing emails
                return -1;
            }
        }

        public int NumberOfIncomingAttachments
        {
            get
            {
                //attached to incoming emails
                return -1;
            }
        }
    }
}

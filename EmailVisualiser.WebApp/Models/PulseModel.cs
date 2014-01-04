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
                return this._data.OutgoingEmails.Select(e => e.Sender).Distinct().Count();
            }
        }

        public int NumberOfOutgoingEmails
        {
            get
            {
                return this._data.OutgoingEmails.Count();
            }
        }

        public int NumberOfExternalRecipients
        {
            get
            {
                return this._data.OutgoingEmails.SelectMany(e => e.Recipients).Distinct().Count();
            }
        }

        public int NumberOfIncomingEmails
        {
            get
            {
                return this._data.IncomingEmails.Count();
            }
        }

        public int NumberOfExternalSenders
        {
            get
            {
                return this._data.IncomingEmails.Select(e => e.Sender).Distinct().Count();
            }
        }

        public int NumberOfInternalEmails
        {
            get
            {
                return this._data.InternalEmails.Count();
            }
        }

        public int NumberOfOutgoingAttachments
        {
            get
            {
                return this._data.OutgoingEmails.Sum(e => e.Attachments);
            }
        }

        public int NumberOfIncomingAttachments
        {
            get
            {
                return this._data.IncomingEmails.Sum(e => e.Attachments);
            }
        }

        public int NumberOfInternalAttachments
        {
            get
            {
                return this._data.InternalEmails.Sum(e => e.Attachments);
            }
        }

        public IEnumerable<Tuple<string, int>> InternalCountPerSender()
        {
            var internalEmailsGroupedBySender = this._data.InternalEmails.GroupBy(e => e.Sender)
                .OrderByDescending(grp => grp.Count());

            return internalEmailsGroupedBySender.Select(grp =>
            {
                return new Tuple<string, int>(grp.Key, grp.Count());
            });
        }

        public IEnumerable<Tuple<string, int>> OutgoingCountPerSender()
        {
            var outgoingEmailsGroupedBySender = this._data.OutgoingEmails.GroupBy(e => e.Sender)
                .OrderByDescending(grp => grp.Count());
            
            return outgoingEmailsGroupedBySender.Select(grp =>
            {
                return new Tuple<string, int>(grp.Key, grp.Count());
            });
        }


        public IEnumerable<string> BusiestConversations
        {
            get
            {
                var convos = this._data.AllEmails.GroupBy(e => e.Subject)
                    .OrderByDescending(grp => grp.Count());

                return convos.Take(10).Select(e => e.Key);
            }
        }
    }
}

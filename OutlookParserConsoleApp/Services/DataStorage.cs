using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParser;

namespace OutlookParserConsoleApp.Services
{
    public class DataStorage
    {
        const string connectionString = "type=embedded;storesdirectory=.\\;storename=Emails";

        protected MyEntityContext NewContext()
        {
            return new MyEntityContext(connectionString);
        }

        public int Store(IEnumerable<Email> emails)
        {
            var ctx = this.NewContext();

            int count = 0;
            foreach (Email email in emails)
            {
                IPersistentEmail persistentEmail = ctx.PersistentEmails.Create();
                persistentEmail.Subject = email.Subject;
                persistentEmail.ReceivedTime = email.ReceivedTime;
                count++;
            }

            ctx.SaveChanges();
            return count;
        }

        public IEnumerable<IPersistentEmail> AllEmails
        {
            get
            {
                var ctx = this.NewContext();

                return ctx.PersistentEmails;
            }
        }

        public int Count
        {
            get
            {
                var ctx = this.NewContext();
                return ctx.PersistentEmails.Count();
            }
        }
    }
}

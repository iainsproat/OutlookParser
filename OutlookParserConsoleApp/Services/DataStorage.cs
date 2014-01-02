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

        public void Store(IEnumerable<Email> emails)
        {
            var ctx = new MyEntityContext(connectionString);

            foreach (Email email in emails)
            {
                IPersistentEmail persistentEmail = ctx.PersistentEmails.Create();
                persistentEmail.Subject = email.Subject;
                persistentEmail.ReceivedTime = email.ReceivedTime;
            }

            ctx.SaveChanges();
        }

        public IEnumerable<IPersistentEmail> AllEmails()
        {
            var ctx = new MyEntityContext(connectionString);

            return ctx.PersistentEmails;
        }
    }
}

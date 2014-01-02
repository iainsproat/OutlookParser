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
            return new MyEntityContext(connectionString); //FIXME should we save just a single context in a private variable and use it repeatedly?
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emails"></param>
        /// <returns>The number of emails stored.</returns>
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

        public void DeleteAll()
        {
            var ctx = this.NewContext();
            var allEmails = this.GetAllEmails(ctx);
            foreach (var email in allEmails)
            {
                ctx.DeleteObject(email);
            }

            ctx.SaveChanges();
        }

        public IEnumerable<IPersistentEmail> AllEmails
        {
            get
            {
                var ctx = this.NewContext();
                return this.GetAllEmails(ctx);
            }
        }

        public IEnumerable<IPersistentEmail> GetAllEmails(MyEntityContext ctx)
        {
            return ctx.PersistentEmails;
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

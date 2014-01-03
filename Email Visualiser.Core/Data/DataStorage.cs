using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

namespace EmailVisualiser.Data
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
        public int Store(IEnumerable<IPersistentEmail> emails)
        {
            var ctx = this.NewContext();

            int count = 0;
            foreach (IPersistentEmail email in emails)
            {
                IPersistentEmail persistentEmail = ctx.PersistentEmails.Create();
                Mapper.Map<IPersistentEmail, IPersistentEmail>(email, persistentEmail); //copy across data in the properties
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

        protected IEnumerable<IPersistentEmail> GetAllEmails(MyEntityContext ctx)
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

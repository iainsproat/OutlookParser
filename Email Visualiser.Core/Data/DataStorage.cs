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

        static DataStorage()
        {
            Mapper.CreateMap<IPersistentEmail, IPersistentEmail>()
                .ForMember(dest => dest.ReceivedTime, opt => opt.MapFrom(src => src.ReceivedTime))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject));
        }

        protected MyEntityContext NewContext()
        {
            return new MyEntityContext(connectionString); //FIXME should we save just a single context in a private variable and use it repeatedly?
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emails"></param>
        /// <returns>The number of emails stored.</returns>
        public int Save(IEnumerable<IPersistentEmail> emails)
        {
            var ctx = this.NewContext();

            int count = 0;
            foreach (IPersistentEmail email in emails)
            {
                IPersistentEmail persistentEmail = ctx.PersistentEmails.Create();
                Mapper.Map<IPersistentEmail, IPersistentEmail>(email, persistentEmail);
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
            Console.WriteLine("You just deleted all the things! You crazy."); //TODO change to logging statement
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

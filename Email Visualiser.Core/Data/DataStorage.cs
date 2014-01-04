using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AutoMapper;

namespace EmailVisualiser.Data
{
    public class DataStorage
    {
        const string pattern = @"[\d\w\.]+@ramboll(\.\w+)+";
        const string connectionString = "type=embedded;storesdirectory=.\\;storename=Emails";

        static DataStorage()
        {
            Mapper.CreateMap<IPersistentEmail, IPersistentEmail>()
                .ForMember(dest => dest.ReceivedTime, opt => opt.MapFrom(src => src.ReceivedTime))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
                .ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => src.Recipients));
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

        public IEnumerable<IPersistentEmail> OutgoingEmails
        {
            get
            {
                var ctx = this.NewContext();
                return ctx.PersistentEmails.Where(e => IsOutgoingEmail(e));
            }
        }

        public IEnumerable<IPersistentEmail> IncomingEmails
        {
            get
            {
                var ctx = this.NewContext();
                return ctx.PersistentEmails.Where(e => IsIncomingEmail(e));
            }
        }

        public IEnumerable<IPersistentEmail> InternalEmails
        {
            get
            {
                var ctx = this.NewContext();
                return ctx.PersistentEmails.Where(e => IsInternalEmail(e));
            }
        }

        protected bool IsOutgoingEmail(IPersistentEmail email)
        {
            return IsInternalSender(email) && HasExternalRecipients(email);
        }

        protected bool IsIncomingEmail(IPersistentEmail email)
        {
            return !IsInternalSender(email);
        }

        protected bool IsInternalEmail(IPersistentEmail email)
        {
            return IsInternalSender(email) && !HasExternalRecipients(email);
        }

        protected bool HasExternalRecipients(IPersistentEmail email)
        {
            return email.Recipients.Any(recipient => !IsInternalEmailAddress(recipient));
        }

        protected bool IsInternalSender(IPersistentEmail email)
        {
            return IsInternalEmailAddress(email.Sender);
        }

        protected bool IsInternalEmailAddress(string emailAddress)
        {
            return Regex.IsMatch(emailAddress, pattern);
        }

        protected IEnumerable<IPersistentEmail> GetAllEmails(MyEntityContext ctx)
        {
            return ctx.PersistentEmails;
        }

        public int TotalNumberOfEmails
        {
            get
            {
                var ctx = this.NewContext();
                return ctx.PersistentEmails.Count();
            }
        }
    }
}

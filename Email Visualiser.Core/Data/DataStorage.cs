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
                .ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => new List<string>(src.Recipients))) //FIXME - this line is flawed...
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));
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
                //Mapper.Map<IPersistentEmail, IPersistentEmail>(email, persistentEmail); //FIXME - this does not correctly copy across the Recipient property

                //HACK the below properties are manually mapped, as there is a problem with AutoMapper correctly transferring the Recipients ICollection property
                persistentEmail.ReceivedTime = email.ReceivedTime;
                persistentEmail.Subject = email.Subject;
                persistentEmail.Sender = email.Sender;
                persistentEmail.Attachments = email.Attachments;
                persistentEmail.Recipients = new List<string>(email.Recipients);

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
                return this.Where(IsOutgoingEmail);
            }
        }

        public IEnumerable<IPersistentEmail> IncomingEmails
        {
            get
            {
                return this.Where(IsIncomingEmail);
            }
        }

        public IEnumerable<IPersistentEmail> InternalEmails
        {
            get
            {
                return this.Where(IsInternalEmail);
            }
        }

        protected IEnumerable<IPersistentEmail> Where(Func<IPersistentEmail, bool> emailFilter)
        {
            var ctx = this.NewContext();
            //BrightStarDb doesn't allow MethodCallExpressions (the Linq is converted to Sparql queries, so invoking the function doesn't seem to be possible), so we need to filter the hard way.
            IList<IPersistentEmail> results = new List<IPersistentEmail>();
            foreach (var email in ctx.PersistentEmails)
            {
                if (emailFilter(email))
                {
                    results.Add(email);
                }
            }

            return results;
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
            if(emailAddress == null)
            {
                emailAddress = string.Empty; //prevent any nasty ArgumentNullExceptions
            }

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

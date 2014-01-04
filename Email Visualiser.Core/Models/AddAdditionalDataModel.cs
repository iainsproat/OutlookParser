using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using OutlookParser;
using EmailVisualiser.Data;

namespace EmailVisualiser.Models
{
    public class AddAdditionalDataModel : IModel
    {
        private readonly DataStorage _data;
        public event Action<string> PathToPstFilesUpdated;
        public event Action<IEnumerable<Email>> FoundEmails;

        static AddAdditionalDataModel()
        {
            Mapper.CreateMap<Email, IPersistentEmail>()
                .ForMember(dest => dest.ReceivedTime, opt => opt.MapFrom(src => src.ReceivedTime))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject));
        }

        public AddAdditionalDataModel(DataStorage storage)
        {
            this._data = storage;
        }

        protected void RaisePathToPstFilesUpdated(string path)
        {
            if (PathToPstFilesUpdated != null)
            {
                PathToPstFilesUpdated(path);
            }
        }

        protected void RaiseFoundEmails(IEnumerable<Email> emails)
        {
            if (FoundEmails != null)
            {
                FoundEmails(emails);
            }
        }

        public DataStorage Data
        {
            get
            {
                return this._data;
            }
        }

        public IEnumerable<Email> ParsePathToPstFiles(string userPath)
        {
            if (File.Exists(userPath))
            {
                RaisePathToPstFilesUpdated(userPath);
                Console.WriteLine("You provided a path to a single .Pst file.");
                return OpenStoreAndExtractMailItems(userPath);
            }
            else if (Directory.Exists(userPath))
            {
                RaisePathToPstFilesUpdated(userPath);
                return FindAndProcessIndividualPstFiles(userPath);
            }
            else
            {
                throw new PstPathException(userPath, new DirectoryNotFoundException(string.Format("A .Pst file or directory containing .Pst files was not found at the given location: '{0}'", userPath)));
            }
        }

        protected IEnumerable<Email> FindAndProcessIndividualPstFiles(string userPath)
        {
            string[] files = Directory.GetFiles(userPath, "*.pst", SearchOption.AllDirectories);

            if (files == null || files.Length < 1)
            {
                throw new PstPathException(userPath, string.Format("No .pst files were found at the given path: {0}", userPath));
            }

            Console.WriteLine("Found the following .pst files:");

            IList<Email> extractedEmails = new List<Email>();
            foreach (string filePath in files)
            {
                Console.WriteLine(filePath);
                IEnumerable<Email> parsedEmails = this.OpenStoreAndExtractMailItems(filePath);
                foreach(var email in parsedEmails)
                {
                    extractedEmails.Add(email);
                }
            }

            return extractedEmails;
        }

        protected IEnumerable<Email> OpenStoreAndExtractMailItems(string userPath)
        {
            PstFile pstFile = new PstFile(userPath);
            IEnumerable<Email> parsedEmails = pstFile.AllItems;

            pstFile.UnloadAndDisconnect();

            RaiseFoundEmails(parsedEmails);

            // persist emails in the database
            var emailsToStore = parsedEmails.Select(email => {
                    return Mapper.Map<Email, IPersistentEmail>(email);
                });
            int numberOfEmailsStored = this._data.Save(emailsToStore);
            Console.WriteLine("Stored all {0} emails from file: {1}", numberOfEmailsStored, pstFile.Path);

            return parsedEmails;
        }

        public int NumberOfExistingEmails
        {
            get
            {
                return this._data.Count;
            }
        }
    }
}

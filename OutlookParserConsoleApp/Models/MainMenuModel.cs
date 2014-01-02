using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Outlook = Microsoft.Office.Interop.Outlook; //TODO remove

using OutlookParser;

namespace OutlookParserConsoleApp.Models
{
    public class MainMenuModel : IModel
    {
        public event Action<string> PathToPstFilesUpdated;

        protected void RaisePathToPstFilesUpdated(string path)
        {
            if (PathToPstFilesUpdated != null)
            {
                PathToPstFilesUpdated(path);
            }
        }

        public void ParsePathToPstFiles(string userPath)
        {
            if (File.Exists(userPath))
            {
                RaisePathToPstFilesUpdated(userPath);
                Console.WriteLine("You provided a path to a single .Pst file.");
                OpenStoreAndExtractMailItems(userPath);
            }
            else if(Directory.Exists(userPath))
            {
                RaisePathToPstFilesUpdated(userPath);
                FindAndProcessIndividualPstFiles(userPath);
            }
            else
            {
                throw new PstPathException(userPath, new DirectoryNotFoundException(string.Format("A .Pst file or directory containing .Pst files was not found at the given location: '{0}'", userPath)));
            }
        }

        public void Test()
        {
            Outlook.Application Application = new Outlook.Application();
            Outlook.Stores stores = Application.Session.Stores;
            foreach (Outlook.Store store in stores)
            {
                if (store.IsDataFileStore == true)
                {
                    Debug.WriteLine(String.Format("Store: "
                        + store.DisplayName
                        + "\n" + "File Path: "
                    + store.FilePath + "\n"));
                }
            }
        }

        public void FindAndProcessIndividualPstFiles(string userPath)
        {
            string[] files = Directory.GetFiles(userPath, "*.pst", SearchOption.AllDirectories);

            if(files == null || files.Length < 1)
            {
                throw new PstPathException(userPath, string.Format("No .pst files were found at the given path: {0}", userPath));
            }

            Console.WriteLine("Found the following .pst files:");
            foreach(string filePath in files)
            {
                Console.WriteLine(filePath);
                this.OpenStoreAndExtractMailItems(filePath);
            }
        }

        public void OpenStoreAndExtractMailItems(string userPath)
        {
            PstFile pstFile = new PstFile(userPath);
            Console.WriteLine("Found a folder : {0}", pstFile.RootFolder.Name);

            IEnumerable<Outlook.MailItem> mailItems = pstFile.AllItems;

            foreach (Outlook.MailItem item in mailItems)
            {
                Console.WriteLine("Email subject : {0}", item.Subject);
            }

            pstFile.Logoff();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Redemption;

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
            RDOPstStore store = pstFile.Store;
            RDOFolder folder = store.IPMRootFolder;
            Console.WriteLine("Found a folder : {0}", folder.Name);
//            RDOFolder defaultInboxFolder = store.GetDefaultFolder(rdoDefaultFolders.olFolderInbox);
//            Console.WriteLine("Found a default folder for inbox : {0}", defaultInboxFolder.Name);
        }
    }
}

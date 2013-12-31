using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ParsePathToPstFile(userPath);
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

        public void ParsePathToPstFile(string userPath)
        {
            Console.WriteLine("You provided a path to a single .Pst file.");
            
        }

        public void FindAndProcessIndividualPstFiles(string userPath)
        {
            string[] files = Directory.GetFiles(userPath, "*.pst", SearchOption.AllDirectories);
            Console.WriteLine("Found the following .pst files:");
            foreach(string filePath in files)
            {
                Console.WriteLine(filePath);
            }
        }
    }
}

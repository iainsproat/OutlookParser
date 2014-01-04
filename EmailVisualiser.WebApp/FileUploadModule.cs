using System;
using System.Collections.Generic;
using System.IO;

using EmailVisualiser.WebApp.Models;
using Nancy;

namespace EmailVisualiser.WebApp
{
    public class FileUploadModule : NancyModule
    {
        public FileUploadModule(IRootPathProvider pathProvider)
            : base("/FileUpload")
        {
            Get["/"] = parameters =>
                {
                    return View["FileUpload.cshtml", new FileUploadModel()];
                };

            Post["/"] = x =>
            {
                var files = this.Request.Files;
                IList<string> fileDetails = new List<string>();

                foreach (var file in files)
                {
                    if (file == null)
                    {
                        continue;
                    }

                    string fileLocationOnServer = SaveFile(file, pathProvider);
                    //TODO check the file is valid
                    fileDetails.Add(string.Format("{3} - {0} ({1}) {2}bytes", file.Name, file.ContentType, file.Value.Length, file.Key));

                    //TODO use the file
                    //TODO dispose of the file after use (?)
                }

                var model = new FileUploadModel();
                model.Posted = fileDetails;
                return View["FileUpload.cshtml", model];
            };
        }

        protected string SaveFile(HttpFile file, IRootPathProvider pathProvider)
        {
            var uploadDirectory = Path.Combine(pathProvider.GetRootPath(), "Content", "uploads"); //FIXME not user specific!

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var filename = Path.Combine(uploadDirectory, file.Name); //FIXME 
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                file.Value.CopyTo(fileStream);
            }

            return filename;
        }
    }
}

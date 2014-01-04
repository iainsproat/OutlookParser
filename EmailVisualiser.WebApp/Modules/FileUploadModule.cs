using System;
using System.Collections.Generic;
using System.IO;

using EmailVisualiser.Models;
using EmailVisualiser.WebApp.Models;
using Nancy;

namespace EmailVisualiser.WebApp
{
    public class FileUploadModule : NancyModule
    {
        public FileUploadModule(IRootPathProvider pathProvider, AddAdditionalDataModel addAdditionalDataModel)
            : base("/FileUpload")
        {
            Get["/"] = parameters =>
                {
                    return View["FileUpload", new FileUploadModel()];
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

                    //TODO use the file
                    addAdditionalDataModel.ParsePathToPstFiles(fileLocationOnServer);

                    fileDetails.Add(string.Format("{3} - {0} ({1}) {2}bytes", file.Name, file.ContentType, file.Value.Length, file.Key));
                    //TODO dispose of the file after use (?)
                }

                var model = new FileUploadModel();
                model.Posted = fileDetails;
                return View["FileUpload", model];
            };
        }

        protected string SaveFile(HttpFile file, IRootPathProvider pathProvider)
        {
            var uploadDirectory = Path.Combine(pathProvider.GetRootPath(), "Content", "uploads");

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var filename = Path.Combine(uploadDirectory, System.Guid.NewGuid().ToString() + file.Name); //HACK creates a unique file name by prepending a Guid
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                file.Value.CopyTo(fileStream);
            }

            return filename;
        }
    }
}

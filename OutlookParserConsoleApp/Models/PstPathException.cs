using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookParserConsoleApp.Models
{
    class PstPathException : Exception
    {
        public PstPathException(string path)
            :this(path, null)
        {
            // empty
        }

        public PstPathException(string path, Exception innerException)
            :base(string.Format("PstPathException.  Could not find a Pst file or a folder containing Pst files at: {0}", path), innerException)
        {
            this.PstPath = path;
        }

        public string PstPath
        {
            get;
            private set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookParser
{
    public class Email
    {
        public string Subject { get; set; }
        public DateTime ReceivedTime { get; set; }
        public string Sender { get; set; }
        public IList<string> Recipients { get; set; }
    }
}

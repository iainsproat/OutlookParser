using System;
using System.Collections.Generic;

using BrightstarDB.EntityFramework;

namespace EmailVisualiser.Data
{
    [Entity]
    public interface IPersistentEmail
    {
        string Subject { get; set; }
        DateTime ReceivedTime { get; set; }
        string Sender { get; set; }
        ICollection<string> Recipients { get; set; }
    }
}

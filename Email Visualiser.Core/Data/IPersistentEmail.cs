using System;

using BrightstarDB.EntityFramework;

namespace EmailVisualiser.Data
{
    [Entity]
    public interface IPersistentEmail
    {
        string Subject { get; set; }
        DateTime ReceivedTime { get; set; }
    }
}

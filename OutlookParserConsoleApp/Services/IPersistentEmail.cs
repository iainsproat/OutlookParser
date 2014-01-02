using System;

using BrightstarDB.EntityFramework;

namespace OutlookParserConsoleApp.Services
{
    [Entity]
    public interface IPersistentEmail
    {
        string Subject { get; set; }
        DateTime ReceivedTime { get; set; }
    }
}

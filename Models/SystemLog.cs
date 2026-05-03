using System;

namespace WeighBridge.Models
{
    public class SystemLog
    {
        public int Id { get; set; }
        public string Event { get; set; } = string.Empty; // Online | Offline | Error
        public string Source { get; set; } = string.Empty; // WB-01 | Zodiac | App
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
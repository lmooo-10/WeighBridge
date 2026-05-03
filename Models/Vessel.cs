using System;

namespace WeighBridge.Models
{
    public class Vessel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public int TEU { get; set; }
        public DateTime ETA { get; set; }
        public string Status { get; set; } = "Import"; // Inbound | Docked | Departed
    }
}

namespace WeighBridge.Models
{
    public class ContainerTypeStat
    {
        public string ContainerType { get; set; } = string.Empty;
        public string WeighmentType { get; set; } = string.Empty;
        public int Count { get; set; }
        public int ImportCount { get; set; }
        public int ExportCount { get; set; }
    }
}
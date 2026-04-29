namespace WeighBridge.Models
{
    public enum WeighmentType   { Inbound, Outbound }
    public enum WeighmentStatus { Pending, Completed, Cancelled }

    public class WeighmentModel
    {
        public string          TicketNumber     { get; set; } = string.Empty;
        public DateTime        DateTime         { get; set; }
        public string          VehicleNumber    { get; set; } = string.Empty;
        public string          DriverName       { get; set; } = string.Empty;
        public string?         ContainerNumber  { get; set; }
        public string          Material         { get; set; } = string.Empty;
        public string          Supplier         { get; set; } = string.Empty;
        public double          GrossWeight      { get; set; }
        public double          TareWeight       { get; set; }
        public double          NetWeight        => GrossWeight - TareWeight;
        public WeighmentType   Type             { get; set; }
        public WeighmentStatus Status           { get; set; }
        public string          OperatorId       { get; set; } = string.Empty;
    }
}

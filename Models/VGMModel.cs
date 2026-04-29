namespace WeighBridge.Models
{
    public enum VGMMethod { Method1, Method2 }
    public enum VGMStatus { Certified, Pending }

    public class VGMModel
    {
        public string    ContainerNumber { get; set; } = string.Empty;
        public double    VGMWeight       { get; set; }
        public VGMMethod Method          { get; set; }
        public string    SealNumber      { get; set; } = string.Empty;
        public string    ShippingLine    { get; set; } = string.Empty;
        public string    BookingNumber   { get; set; } = string.Empty;
        public string    VerifiedBy      { get; set; } = string.Empty;
        public DateTime  Date            { get; set; }
        public VGMStatus Status          { get; set; }
    }
}

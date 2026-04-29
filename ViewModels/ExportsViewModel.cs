using WeighBridge.ViewModels.Base;

namespace WeighBridge.ViewModels
{
    public class ExportsViewModel : BaseViewModel
    {
        // ── Dropdowns ─────────────────────────────────────────
        private string _exportType = "Weighments";
        private string _dateRange  = "All Time";
        private string _format     = "CSV";

        public string ExportType { get => _exportType; set => SetProperty(ref _exportType, value); }
        public string DateRange  { get => _dateRange;  set => SetProperty(ref _dateRange,  value); }
        public string Format     { get => _format;     set => SetProperty(ref _format,     value); }

        public List<string> ExportTypes { get; } = new() { "Weighments", "VGM Certificates", "Both" };
        public List<string> DateRanges  { get; } = new() { "All Time", "Today", "Last 7 Days", "Last 30 Days" };
        public List<string> Formats     { get; } = new() { "CSV", "Excel (XLSX)", "PDF" };

        // ── Stats (read-only display) ─────────────────────────
        public int    WeighmentsAvailable { get; } = 5;
        public int    VGMCertificates     { get; } = 2;
        public string TotalWeight         { get; } = "73.70 MT";

        // ── Column checkboxes — FIXED: backing fields + SetProperty ──
        // CheckBox.IsChecked binds TwoWay by default.
        // Properties without a setter crash with "read-only property" error.
        private bool _colTicketNumber    = true;
        private bool _colDateTime        = true;
        private bool _colVehicleNumber   = true;
        private bool _colDriverName      = true;
        private bool _colContainerNumber = true;
        private bool _colMaterial        = true;
        private bool _colSupplier        = true;
        private bool _colWeightDetails   = true;
        private bool _colType            = true;
        private bool _colStatus          = true;

        public bool ColTicketNumber
        {
            get => _colTicketNumber;
            set => SetProperty(ref _colTicketNumber, value);
        }
        public bool ColDateTime
        {
            get => _colDateTime;
            set => SetProperty(ref _colDateTime, value);
        }
        public bool ColVehicleNumber
        {
            get => _colVehicleNumber;
            set => SetProperty(ref _colVehicleNumber, value);
        }
        public bool ColDriverName
        {
            get => _colDriverName;
            set => SetProperty(ref _colDriverName, value);
        }
        public bool ColContainerNumber
        {
            get => _colContainerNumber;
            set => SetProperty(ref _colContainerNumber, value);
        }
        public bool ColMaterial
        {
            get => _colMaterial;
            set => SetProperty(ref _colMaterial, value);
        }
        public bool ColSupplier
        {
            get => _colSupplier;
            set => SetProperty(ref _colSupplier, value);
        }
        public bool ColWeightDetails
        {
            get => _colWeightDetails;
            set => SetProperty(ref _colWeightDetails, value);
        }
        public bool ColType
        {
            get => _colType;
            set => SetProperty(ref _colType, value);
        }
        public bool ColStatus
        {
            get => _colStatus;
            set => SetProperty(ref _colStatus, value);
        }

        // ── Command ───────────────────────────────────────────
        public RelayCommand ExportDataCommand { get; }

        public ExportsViewModel()
        {
            ExportDataCommand = new RelayCommand(_ =>
                System.Windows.MessageBox.Show(
                    $"Exporting {ExportType} as {Format}...", "Export Data"));
        }
    }
}

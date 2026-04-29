using WeighBridge.ViewModels.Base;

namespace WeighBridge.ViewModels
{
    public class ReportsViewModel : BaseViewModel
    {
        private string _reportType = "Daily Report";
        private string _period     = "Current Period";

        public string ReportType { get => _reportType; set => SetProperty(ref _reportType, value); }
        public string Period     { get => _period;     set => SetProperty(ref _period,     value); }

        public List<string> ReportTypes { get; } = new() { "Daily Report", "Weekly Report", "Monthly Report", "Custom Range" };
        public List<string> Periods     { get; } = new() { "Current Period", "Last 7 Days", "Last 30 Days", "All Time" };

        // Stats
        public int    TotalWeighments { get; } = 4;
        public string TotalWeight     { get; } = "73.70 MT";
        public string AverageWeight   { get; } = "18.43 MT";
        public int    Pending         { get; } = 1;

        public RelayCommand DownloadReportCommand { get; }

        public ReportsViewModel()
        {
            DownloadReportCommand = new RelayCommand(_ =>
                System.Windows.MessageBox.Show("Report download triggered.", "Download Report"));
        }
    }
}

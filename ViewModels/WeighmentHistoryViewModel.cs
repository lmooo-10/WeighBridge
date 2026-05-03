using WeighBridge.Models;
using WeighBridge.ViewModels.Base;
using System.Collections.ObjectModel;

namespace WeighBridge.ViewModels
{
    public class WeighmentHistoryViewModel : BaseViewModel
    {
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ApplyFilter(); }
        }

        public ObservableCollection<WeighmentModel> AllRecords    { get; } = new();
        public ObservableCollection<WeighmentModel> FilteredRecords { get; } = new();

        public RelayCommand ExportCsvCommand { get; }

        public WeighmentHistoryViewModel()
        {
            ExportCsvCommand = new RelayCommand(_ => ExportCsv());
            LoadSampleData();
            ApplyFilter();
        }

        private void LoadSampleData()
        {
            AllRecords.Add(new WeighmentModel { TicketNumber="WB-2026-003", DateTime=new DateTime(2026,4,6,10,0,0),  VehicleNumber="DXB-T-24680", DriverName="Khalid Ibrahim",   Material="Electronics",             Supplier="Gulf Steel Industries", GrossWeight=15.0, TareWeight=0.0,  Type=WeighmentType.Import,  Status=WeighmentStatus.Pending   });
            AllRecords.Add(new WeighmentModel { TicketNumber="WB-2026-002", DateTime=new DateTime(2026,4,6, 9,15,0), VehicleNumber="DXB-T-67890", DriverName="Mohammed Ali",     Material="Construction Materials",  Supplier="Construction Co.",      GrossWeight=32.0, TareWeight=9.0,  Type=WeighmentType.Export, Status=WeighmentStatus.Completed });
            AllRecords.Add(new WeighmentModel { TicketNumber="WB-2026-001", DateTime=new DateTime(2026,4,6, 8,30,0), VehicleNumber="DXB-T-12345", DriverName="Ahmed Hassan",     Material="Steel Coils",             Supplier="Gulf Steel Industries", GrossWeight=28.5, TareWeight=8.5,  Type=WeighmentType.Import,  Status=WeighmentStatus.Completed });
            AllRecords.Add(new WeighmentModel { TicketNumber="WB-2026-004", DateTime=new DateTime(2026,4,5, 2,20,0), VehicleNumber="DXB-T-13579", DriverName="Rashid Omar",      Material="Automotive Parts",        Supplier="Parts Trading LLC",     GrossWeight=26.0, TareWeight=7.5,  Type=WeighmentType.Import,  Status=WeighmentStatus.Completed });
            AllRecords.Add(new WeighmentModel { TicketNumber="WB-2026-005", DateTime=new DateTime(2026,4,4,11,45,0), VehicleNumber="DXB-T-98765", DriverName="Abdullah Fahad",   Material="Textiles",                Supplier="Fabric World DMCC",     GrossWeight=19.0, TareWeight=6.8,  Type=WeighmentType.Export, Status=WeighmentStatus.Completed });
        }

        private void ApplyFilter()
        {
            FilteredRecords.Clear();
            var q = SearchText.ToLower();
            foreach (var r in AllRecords)
                if (string.IsNullOrEmpty(q) ||
                    r.TicketNumber.ToLower().Contains(q) ||
                    r.VehicleNumber.ToLower().Contains(q) ||
                    r.DriverName.ToLower().Contains(q))
                    FilteredRecords.Add(r);
        }

        private void ExportCsv()
        {
            // Placeholder – wire to SaveFileDialog in code-behind
            System.Windows.MessageBox.Show("CSV Export triggered.", "Export");
        }
    }
}

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

        public ObservableCollection<WeighmentModel> AllRecords { get; } = new();
        public ObservableCollection<WeighmentModel> FilteredRecords { get; } = new();

        public RelayCommand ExportCsvCommand { get; }

        public WeighmentHistoryViewModel()
        {
            ExportCsvCommand = new RelayCommand(_ => ExportCsv());
            ApplyFilter();
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
            System.Windows.MessageBox.Show("CSV Export triggered.", "Export");
        }
    }
}
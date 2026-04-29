using WeighBridge.Models;
using WeighBridge.ViewModels.Base;
using System.Collections.ObjectModel;

namespace WeighBridge.ViewModels
{
    public class VGMViewModel : BaseViewModel
    {
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ApplyFilter(); }
        }

        public int TotalVGMIssued { get; } = 2;
        public int TodaysVGM     { get; } = 0;
        public int Method1Count  { get; } = 1;
        public int Method2Count  { get; } = 1;

        public ObservableCollection<VGMModel> AllCertificates      { get; } = new();
        public ObservableCollection<VGMModel> FilteredCertificates { get; } = new();

        public RelayCommand IssueCertificateCommand { get; }

        public VGMViewModel()
        {
            IssueCertificateCommand = new RelayCommand(_ =>
                System.Windows.MessageBox.Show("Issue VGM Certificate dialog — coming soon.", "VGM"));
            LoadSampleData();
            ApplyFilter();
        }

        private void LoadSampleData()
        {
            AllCertificates.Add(new VGMModel
            {
                ContainerNumber = "DPWU7654321", VGMWeight = 20.00, Method = VGMMethod.Method1,
                SealNumber = "SL789456", ShippingLine = "Maersk Line", BookingNumber = "MAE2026001",
                VerifiedBy = "John Smith", Date = new DateTime(2026, 4, 6, 9, 0, 0), Status = VGMStatus.Certified
            });
            AllCertificates.Add(new VGMModel
            {
                ContainerNumber = "DPWU1234567", VGMWeight = 18.50, Method = VGMMethod.Method2,
                SealNumber = "SL654321", ShippingLine = "MSC", BookingNumber = "MSC2026045",
                VerifiedBy = "Sarah Johnson", Date = new DateTime(2026, 4, 5, 15, 0, 0), Status = VGMStatus.Certified
            });
        }

        private void ApplyFilter()
        {
            FilteredCertificates.Clear();
            var q = SearchText.ToLower();
            foreach (var c in AllCertificates)
                if (string.IsNullOrEmpty(q) || c.ContainerNumber.ToLower().Contains(q))
                    FilteredCertificates.Add(c);
        }
    }
}

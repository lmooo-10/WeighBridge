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

        public int TotalVGMIssued { get => AllCertificates.Count; }
        public int TodaysVGM { get => AllCertificates.Count(c => c.Date.Date == DateTime.Today); }

        public ObservableCollection<VGMModel> AllCertificates { get; } = new();
        public ObservableCollection<VGMModel> FilteredCertificates { get; } = new();

        public RelayCommand IssueCertificateCommand { get; }

        public VGMViewModel()
        {
            IssueCertificateCommand = new RelayCommand(_ =>
                System.Windows.MessageBox.Show("Issue VGM Certificate dialog — coming soon.", "VGM"));
            // ── Mock VGM certificates ─────────────────────────────
            AllCertificates.Add(new VGMModel { ContainerNumber = "MSCU1234567", VGMWeight = 24.80, SealNumber = "SL-001-2026", ShippingLine = "MSC", BookingNumber = "MSC-BK-78901", VerifiedBy = "Ahmed Kaci", Date = new DateTime(2026, 4, 6, 8, 30, 0), Status = VGMStatus.Certified });
            AllCertificates.Add(new VGMModel { ContainerNumber = "TCKU3456781", VGMWeight = 18.50, SealNumber = "SL-002-2026", ShippingLine = "CMA CGM", BookingNumber = "CMA-BK-44231", VerifiedBy = "Youcef Brahimi", Date = new DateTime(2026, 4, 6, 9, 15, 0), Status = VGMStatus.Certified });
            AllCertificates.Add(new VGMModel { ContainerNumber = "HLXU9876543", VGMWeight = 31.20, SealNumber = "SL-003-2026", ShippingLine = "Hapag", BookingNumber = "HPG-BK-11203", VerifiedBy = "Ahmed Kaci", Date = new DateTime(2026, 4, 5, 14, 0, 0), Status = VGMStatus.Certified });
            AllCertificates.Add(new VGMModel { ContainerNumber = "OOLU5544321", VGMWeight = 22.40, SealNumber = "SL-004-2026", ShippingLine = "OOCL", BookingNumber = "OOC-BK-98712", VerifiedBy = "Nadia Mansouri", Date = new DateTime(2026, 4, 5, 11, 45, 0), Status = VGMStatus.Pending });
            AllCertificates.Add(new VGMModel { ContainerNumber = "GESU7788990", VGMWeight = 27.60, SealNumber = "SL-005-2026", ShippingLine = "Evergreen", BookingNumber = "EVG-BK-33410", VerifiedBy = "Youcef Brahimi", Date = new DateTime(2026, 4, 4, 16, 20, 0), Status = VGMStatus.Certified });
            AllCertificates.Add(new VGMModel { ContainerNumber = "CSNU4412378", VGMWeight = 15.90, SealNumber = "SL-006-2026", ShippingLine = "COSCO", BookingNumber = "COS-BK-55678", VerifiedBy = "Ahmed Kaci", Date = new DateTime(2026, 4, 4, 10, 0, 0), Status = VGMStatus.Pending });
            ApplyFilter();
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
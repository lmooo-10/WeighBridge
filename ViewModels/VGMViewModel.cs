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
        public int Method1Count { get => AllCertificates.Count(c => c.Method == VGMMethod.Method1); }
        public int Method2Count { get => AllCertificates.Count(c => c.Method == VGMMethod.Method2); }

        public ObservableCollection<VGMModel> AllCertificates { get; } = new();
        public ObservableCollection<VGMModel> FilteredCertificates { get; } = new();

        public RelayCommand IssueCertificateCommand { get; }

        public VGMViewModel()
        {
            IssueCertificateCommand = new RelayCommand(_ =>
                System.Windows.MessageBox.Show("Issue VGM Certificate dialog — coming soon.", "VGM"));
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
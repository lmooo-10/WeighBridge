using WeighBridge.Models;
using WeighBridge.ViewModels.Base;
using System.Collections.ObjectModel;

namespace WeighBridge.ViewModels
{
    public class ContainerTrackingViewModel : BaseViewModel
    {
        // ── Search / Filter ───────────────────────────────────
        private string _searchText = string.Empty;
        private string _selectedFilter = "All";

        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ApplyFilter(); }
        }

        public string SelectedFilter
        {
            get => _selectedFilter;
            set { SetProperty(ref _selectedFilter, value); ApplyFilter(); }
        }

        public ObservableCollection<string> StatusFilter { get; } = new()
        {
            "All",
            "Pending",
            "Weighed",
            "Out"
        };

        // ── KPI counters ──────────────────────────────────────
        private int _totalContainers;
        private int _pendingCount;
        private int _weighedCount;
        private int _exitedCount;

        public int TotalContainers { get => _totalContainers; set => SetProperty(ref _totalContainers, value); }
        public int EnAttenteCount { get => _pendingCount; set => SetProperty(ref _pendingCount, value); }
        public int PeseCount { get => _weighedCount; set => SetProperty(ref _weighedCount, value); }
        public int SortiCount { get => _exitedCount; set => SetProperty(ref _exitedCount, value); }

        // ── Collections ───────────────────────────────────────
        public ObservableCollection<ContainerTrackingModel> AllContainers { get; } = new();
        public ObservableCollection<ContainerTrackingModel> FilteredContainers { get; } = new();

        // ── Selected container (for detail panel) ─────────────
        private ContainerTrackingModel? _selectedContainer;
        public bool HasSelectedContainer => _selectedContainer != null;
        public ContainerTrackingModel? SelectedContainer
        {
            get => _selectedContainer;
            set
            {
                SetProperty(ref _selectedContainer, value);
                OnPropertyChanged(nameof(HasSelectedContainer));
            }
        }

        // ── Commands ──────────────────────────────────────────
        public RelayCommand RefreshCommand { get; }
        public RelayCommand ExportCommand { get; }
        public RelayCommand SelectContainerCommand { get; }

        // ── Constructor ───────────────────────────────────────
        public ContainerTrackingViewModel()
        {
            RefreshCommand = new RelayCommand(_ => Refresh());
            ExportCommand = new RelayCommand(_ => Export());
            SelectContainerCommand = new RelayCommand(p =>
            {
                if (p is ContainerTrackingModel c) SelectedContainer = c;
            });
            // ← Seed sample data so the grid isn't empty
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "MSCU1234567",
                Type = "20' STD",
                Direction = "Import",
                Status = ContainerStatus.Pending,
                VehicleNumber = "16-123-001",
                DriverName = "Ali Benali",
                EntryTime = DateTime.Now.AddHours(-2),
                GrossWeight = 24.5,
                TareWeight = 4.2,
                Location = "Zone A - Ligne 3",
                Remarks = ""
            });
            ApplyFilter();
            UpdateKpis();
        }

        private void ApplyFilter()
        {
            FilteredContainers.Clear();
            var q = SearchText.Trim().ToLower();

            foreach (var c in AllContainers)
            {
                bool matchSearch = string.IsNullOrEmpty(q)
                    || c.ContainerNumber.ToLower().Contains(q)
                    || c.VehicleNumber.ToLower().Contains(q)
                    || c.DriverName.ToLower().Contains(q);

                bool matchStatus = SelectedFilter == "ALL"
                    || c.StatusLabel == SelectedFilter;

                if (matchSearch && matchStatus)
                    FilteredContainers.Add(c);
            }

            OnPropertyChanged(nameof(FilteredContainers));
        }

        private void UpdateKpis()
        {
            TotalContainers = AllContainers.Count;
            EnAttenteCount = AllContainers.Count(c => c.Status == ContainerStatus.Pending);
            PeseCount = AllContainers.Count(c => c.Status == ContainerStatus.Weighed);
            SortiCount = AllContainers.Count(c => c.Status == ContainerStatus.Exited);
        }

        private void Refresh()
        {
            ApplyFilter();
            UpdateKpis();
            System.Windows.MessageBox.Show("Données actualisées.", "Rafraîchir");
        }

        private void Export()
        {
            System.Windows.MessageBox.Show(
                $"Export CSV — {FilteredContainers.Count} conteneurs.",
                "Export");
        }
    }

    // ── Model ─────────────────────────────────────────────────
    public enum ContainerStatus { Pending, Weighed, Exited }

    public class ContainerTrackingModel
    {
        public string ContainerNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
        public ContainerStatus Status { get; set; }
        public string VehicleNumber { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public DateTime EntryTime { get; set; }
        public DateTime? WeighTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public double GrossWeight { get; set; }
        public double TareWeight { get; set; }
        public double NetWeight => GrossWeight - TareWeight;
        public string Location { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;

        // ── Display helpers ───────────────────────────────────
        public string StatusLabel => Status switch
        {
            ContainerStatus.Pending => "Pending",
            ContainerStatus.Weighed => "Weighed",
            ContainerStatus.Exited => "Out",
            _ => "Unknown"
        };

        public string StatusBadgeColor => Status switch
        {
            ContainerStatus.Pending => "#EF9F27",
            ContainerStatus.Weighed => "#3B6D11",
            ContainerStatus.Exited => "#9DA3C0",
            _ => "#8A8E9B"
        };

        public string WeighTimeDisplay =>
            WeighTime.HasValue ? WeighTime.Value.ToString("HH:mm") : "--:--";

        public string ExitTimeDisplay =>
            ExitTime.HasValue ? ExitTime.Value.ToString("HH:mm") : "--:--";

        public string NetWeightDisplay =>
            GrossWeight > 0 ? $"{NetWeight:F2} MT" : "—";

        public string DurationDisplay
        {
            get
            {
                var end = ExitTime ?? DateTime.Now;
                var span = end - EntryTime;
                return span.TotalMinutes < 60
                    ? $"{(int)span.TotalMinutes} min"
                    : $"{(int)span.TotalHours}h {span.Minutes:00}m";
            }
        }
    }
}
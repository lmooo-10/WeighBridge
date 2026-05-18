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
            // ← Seed sample data
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "MSCU1234567",
                Type = "20' STD",
                Direction = "Import",
                Status = ContainerStatus.Pending,
                VehicleNumber = "16-123-001",
                DriverName = "Ali Benali",
                EntryTime = DateTime.Now.AddHours(-2),
                GrossWeight = 0,
                TareWeight = 0,
                Location = "Gate-In WB1",
                Remarks = "Awaiting weighing"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "DPWU7654321",
                Type = "40' STD",
                Direction = "Export",
                Status = ContainerStatus.Weighed,
                VehicleNumber = "09-ORA-4412",
                DriverName = "Mohammed Ali",
                EntryTime = DateTime.Now.AddHours(-4),
                WeighTime = DateTime.Now.AddHours(-3),
                GrossWeight = 32.0,
                TareWeight = 9.0,
                Location = "Export Zone - WB2",
                Remarks = "VGM issued"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "MAEU9988776",
                Type = "20' REEF",
                Direction = "Import",
                Status = ContainerStatus.Out,
                VehicleNumber = "23-CON-7891",
                DriverName = "Ahmed Hassan",
                EntryTime = DateTime.Now.AddHours(-8),
                WeighTime = DateTime.Now.AddHours(-7),
                ExitTime = DateTime.Now.AddHours(-6),
                GrossWeight = 28.5,
                TareWeight = 8.5,
                Location = "Terminal Exit",
                Remarks = "Delivered to client"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "TCKU3344556",
                Type = "20' DGR",
                Direction = "Import",
                Status = ContainerStatus.Pending,
                VehicleNumber = "31-SET-1100",
                DriverName = "Rashid Omar",
                EntryTime = DateTime.Now.AddHours(-1),
                GrossWeight = 0,
                TareWeight = 0,
                Location = "Gate-In WB1",
                Remarks = "Dangerous goods - doc verification"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "HLXU5566778",
                Type = "40' STD",
                Direction = "Export",
                Status = ContainerStatus.Weighed,
                VehicleNumber = "07-ALG-9934",
                DriverName = "Abdullah Fahad",
                EntryTime = DateTime.Now.AddHours(-5),
                WeighTime = DateTime.Now.AddHours(-4),
                GrossWeight = 26.0,
                TareWeight = 7.5,
                Location = "Export Zone - WB2",
                Remarks = "Awaiting embarkation"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "CMAU1122334",
                Type = "20' STD",
                Direction = "Import",
                Status = ContainerStatus.Out,
                VehicleNumber = "14-MED-5523",
                DriverName = "Youcef Bouzid",
                EntryTime = DateTime.Now.AddHours(-10),
                WeighTime = DateTime.Now.AddHours(-9),
                ExitTime = DateTime.Now.AddHours(-8),
                GrossWeight = 19.0,
                TareWeight = 6.8,
                Location = "Terminal Exit",
                Remarks = "Delivered"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "GESU4433221",
                Type = "20' STD",
                Direction = "Export",
                Status = ContainerStatus.Pending,
                VehicleNumber = "02-ALG-3310",
                DriverName = "Karim Meziane",
                EntryTime = DateTime.Now.AddMinutes(-30),
                GrossWeight = 0,
                TareWeight = 0,
                Location = "Gate-In WB1",
                Remarks = "Awaiting weighing"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "OOLU8877665",
                Type = "20' REEF",
                Direction = "Export",
                Status = ContainerStatus.Weighed,
                VehicleNumber = "18-SET-6621",
                DriverName = "Samir Hadj",
                EntryTime = DateTime.Now.AddHours(-6),
                WeighTime = DateTime.Now.AddHours(-5),
                GrossWeight = 22.4,
                TareWeight = 7.1,
                Location = "Export Zone - WB2",
                Remarks = "Reefer plugged in"
            });

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

                bool matchStatus = SelectedFilter == "All"
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
            SortiCount = AllContainers.Count(c => c.Status == ContainerStatus.Out);
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
    public enum ContainerStatus { Pending, Weighed, Out }

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
            ContainerStatus.Out => "Out",
            _ => "Unknown"
        };

        public string StatusBadgeColor => Status switch
        {
            ContainerStatus.Pending => "#EF9F27",
            ContainerStatus.Weighed => "#3B6D11",
            ContainerStatus.Out => "#9DA3C0",
            _ => "#8A8E9B"
        };

        public string WeighTimeDisplay =>
            WeighTime.HasValue ? WeighTime.Value.ToString("HH:mm") : "--:--";

        public string ExitTimeDisplay =>
            ExitTime.HasValue ? ExitTime.Value.ToString("HH:mm") : "--:--";

        public string NetWeightDisplay =>
            GrossWeight > 0 ? $"{NetWeight:F2}" : "—";

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
using WeighBridge.Models;
using WeighBridge.ViewModels.Base;
using System.Collections.ObjectModel;

namespace WeighBridge.ViewModels
{
    public class ContainerTrackingViewModel : BaseViewModel
    {
        // ── Search / Filter ───────────────────────────────────
        private string _searchText = string.Empty;
        private string _statusFilter = "All";

        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ApplyFilter(); }
        }

        public string StatusFilter
        {
            get => _statusFilter;
            set { SetProperty(ref _statusFilter, value); ApplyFilter(); }
        }

        public List<string> StatusFilters { get; } = new()
            { "All", "En attente", "Pesé", "Sorti" };

        // ── KPI counters ──────────────────────────────────────
        private int _totalContainers;
        private int _enAttenteCount;
        private int _peseCount;
        private int _sortiCount;

        public int TotalContainers { get => _totalContainers; set => SetProperty(ref _totalContainers, value); }
        public int EnAttenteCount { get => _enAttenteCount; set => SetProperty(ref _enAttenteCount, value); }
        public int PeseCount { get => _peseCount; set => SetProperty(ref _peseCount, value); }
        public int SortiCount { get => _sortiCount; set => SetProperty(ref _sortiCount, value); }

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

            LoadSampleData();
            ApplyFilter();
            UpdateKpis();
        }

        // ── Sample data (mirrors WeighmentHistoryViewModel style) ─
        private void LoadSampleData()
        {
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "DPWU7654321",
                Type = "20' STD",
                Direction = "Import",
                Status = ContainerStatus.EnAttente,
                VehicleNumber = "16-ALG-2301",
                DriverName = "Khalid Ibrahim",
                EntryTime = new DateTime(2026, 5, 5, 6, 10, 0),
                GrossWeight = 0,
                TareWeight = 0,
                Location = "Gate-In WB1",
                Remarks = "En attente de pesée"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "MSCU1234567",
                Type = "40' STD",
                Direction = "Export",
                Status = ContainerStatus.Pese,
                VehicleNumber = "09-ORA-4412",
                DriverName = "Mohammed Ali",
                EntryTime = new DateTime(2026, 5, 5, 7, 30, 0),
                WeighTime = new DateTime(2026, 5, 5, 7, 45, 0),
                GrossWeight = 32.0,
                TareWeight = 9.0,
                Location = "Zone Export - WB2",
                Remarks = "VGM émis"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "MAEU9988776",
                Type = "20' REEF",
                Direction = "Import",
                Status = ContainerStatus.Sorti,
                VehicleNumber = "23-CON-7891",
                DriverName = "Ahmed Hassan",
                EntryTime = new DateTime(2026, 5, 5, 5, 0, 0),
                WeighTime = new DateTime(2026, 5, 5, 5, 20, 0),
                ExitTime = new DateTime(2026, 5, 5, 6, 5, 0),
                GrossWeight = 28.5,
                TareWeight = 8.5,
                Location = "Sortie terminale",
                Remarks = "Livré au client"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "TCKU3344556",
                Type = "20' DGR",
                Direction = "Import",
                Status = ContainerStatus.EnAttente,
                VehicleNumber = "31-SET-1100",
                DriverName = "Rashid Omar",
                EntryTime = new DateTime(2026, 5, 5, 8, 0, 0),
                GrossWeight = 0,
                TareWeight = 0,
                Location = "File d'attente WB1",
                Remarks = "Matières dangereuses - vérif. docs"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "HLXU5566778",
                Type = "40' STD",
                Direction = "Export",
                Status = ContainerStatus.Pese,
                VehicleNumber = "07-ALG-9934",
                DriverName = "Abdullah Fahad",
                EntryTime = new DateTime(2026, 5, 5, 9, 0, 0),
                WeighTime = new DateTime(2026, 5, 5, 9, 18, 0),
                GrossWeight = 26.0,
                TareWeight = 7.5,
                Location = "Zone Export - WB2",
                Remarks = "Attente embarquement"
            });
            AllContainers.Add(new ContainerTrackingModel
            {
                ContainerNumber = "CMAU1122334",
                Type = "20' STD",
                Direction = "Import",
                Status = ContainerStatus.Sorti,
                VehicleNumber = "14-MED-5523",
                DriverName = "Youcef Bouzid",
                EntryTime = new DateTime(2026, 5, 4, 22, 0, 0),
                WeighTime = new DateTime(2026, 5, 4, 22, 30, 0),
                ExitTime = new DateTime(2026, 5, 4, 23, 10, 0),
                GrossWeight = 19.0,
                TareWeight = 6.8,
                Location = "Sortie terminale",
                Remarks = "Livré"
            });
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

                bool matchStatus = StatusFilter == "All"
                    || c.StatusLabel == StatusFilter;

                if (matchSearch && matchStatus)
                    FilteredContainers.Add(c);
            }

            OnPropertyChanged(nameof(FilteredContainers));
        }

        private void UpdateKpis()
        {
            TotalContainers = AllContainers.Count;
            EnAttenteCount = AllContainers.Count(c => c.Status == ContainerStatus.EnAttente);
            PeseCount = AllContainers.Count(c => c.Status == ContainerStatus.Pese);
            SortiCount = AllContainers.Count(c => c.Status == ContainerStatus.Sorti);
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
    public enum ContainerStatus { EnAttente, Pese, Sorti }

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
            ContainerStatus.EnAttente => "En attente",
            ContainerStatus.Pese => "Pesé",
            ContainerStatus.Sorti => "Sorti",
            _ => "Inconnu"
        };

        public string StatusBadgeColor => Status switch
        {
            ContainerStatus.EnAttente => "#E67E22",
            ContainerStatus.Pese => "#2ECC71",
            ContainerStatus.Sorti => "#9DA3C0",
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
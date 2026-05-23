using WeighBridge.Models;
using WeighBridge.ViewModels.Base;
using System.Collections.ObjectModel;

namespace WeighBridge.ViewModels
{
    public class ReportsViewModel : BaseViewModel
    {
        // ── Filters ───────────────────────────────────────────
        private string _reportType = "Daily Report";
        private string _period = "Today";
        private string _statusFilter = "All";
        private string _searchText = string.Empty;

        public string ReportType
        {
            get => _reportType;
            set { SetProperty(ref _reportType, value); ApplyFilter(); }
        }

        public string Period
        {
            get => _period;
            set { SetProperty(ref _period, value); ApplyFilter(); }
        }

        public string StatusFilter
        {
            get => _statusFilter;
            set { SetProperty(ref _statusFilter, value); ApplyFilter(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ApplyFilter(); }
        }

        public List<string> ReportTypes { get; } = new() { "Daily Report", "Shift Report", "Weekly Report", "Monthly Report" };
        public List<string> Periods { get; } = new() { "Today", "Last 7 Days", "Last 30 Days", "All Time" };
        public List<string> StatusFilters { get; } = new() { "All", "Completed", "Pending", "Cancelled" };

        // ── Summary KPIs ──────────────────────────────────────
        private int _totalWeighments;
        private int _completedCount;
        private int _pendingCount;
        private int _cancelledCount;
        private double _totalNetWeight;
        private double _totalGrossWeight;
        private double _averageNetWeight;
        private int _importCount;
        private int _exportCount;

        public int TotalWeighments { get => _totalWeighments; set => SetProperty(ref _totalWeighments, value); }
        public int CompletedCount { get => _completedCount; set => SetProperty(ref _completedCount, value); }
        public int PendingCount { get => _pendingCount; set => SetProperty(ref _pendingCount, value); }
        public int CancelledCount { get => _cancelledCount; set => SetProperty(ref _cancelledCount, value); }
        public double TotalNetWeight { get => _totalNetWeight; set => SetProperty(ref _totalNetWeight, value); }
        public double TotalGrossWeight { get => _totalGrossWeight; set => SetProperty(ref _totalGrossWeight, value); }
        public double AverageNetWeight { get => _averageNetWeight; set => SetProperty(ref _averageNetWeight, value); }
        public int ImportCount { get => _importCount; set => SetProperty(ref _importCount, value); }
        public int ExportCount { get => _exportCount; set => SetProperty(ref _exportCount, value); }

        // Bar chart scaling
        public int MaxTypeValue => Math.Max(1, Math.Max(ImportCount, ExportCount));

        // ── Shift summary ─────────────────────────────────────
        public List<ShiftSummary> ShiftSummaries { get; } = new()
        {
            new ShiftSummary { ShiftName = "Morning",   Operator = "Ahmed Kaci",   OpsCount = 2, TotalWeight = 48.5, StartTime = "06:00", EndTime = "14:00" },
            new ShiftSummary { ShiftName = "Afternoon", Operator = "Sara Benali",  OpsCount = 1, TotalWeight = 19.0, StartTime = "14:00", EndTime = "22:00" },
            new ShiftSummary { ShiftName = "Night",     Operator = "Karim Oussad", OpsCount = 1, TotalWeight = 26.0, StartTime = "22:00", EndTime = "06:00" },
        };

        // ── Weighment records (master list) ───────────────────
        private readonly List<WeighmentReportRow> _allRows = new();

        // ── Filtered / displayed records ──────────────────────
        public ObservableCollection<WeighmentReportRow> FilteredRows { get; } = new();

        // ── Commands ──────────────────────────────────────────
        public RelayCommand DownloadReportCommand { get; }
        public RelayCommand PrintTicketCommand { get; }
        public RelayCommand ExportCsvCommand { get; }

        // ── Constructor ───────────────────────────────────────
        public ReportsViewModel()
        {
            DownloadReportCommand = new RelayCommand(_ => DownloadReport());
            PrintTicketCommand = new RelayCommand(p => PrintTicket(p as WeighmentReportRow),
                                                     p => p is WeighmentReportRow);
            ExportCsvCommand = new RelayCommand(_ => ExportCsv());

            LoadSampleData();
            ApplyFilter();
        }

        // ── Sample data (mirrors WeighmentHistoryViewModel) ───
        private void LoadSampleData()
        {
            _allRows.Add(new WeighmentReportRow
            {
                TicketNumber = "WB-2026-001",
                Date = new DateTime(2026, 5, 23, 8, 30, 0),
                VehicleNumber = "16-ALG-2301",
                DriverName = "Ahmed Hassan",
                ContainerNumber = "DPWU1234567",
                Material = "Steel Coils",
                Supplier = "Gulf Steel Industries",
                GrossWeight = 28.5,
                TareWeight = 8.5,
                Type = "Import",
                Status = "Completed",
                Operator = "Ahmed Kaci",
                Shift = "Morning"
            });
            _allRows.Add(new WeighmentReportRow
            {
                TicketNumber = "WB-2026-002",
                Date = new DateTime(2026, 5, 23, 9, 15, 0),
                VehicleNumber = "09-ORA-4412",
                DriverName = "Mohammed Ali",
                ContainerNumber = "MSCU7890123",
                Material = "Construction Materials",
                Supplier = "Construction Co.",
                GrossWeight = 32.0,
                TareWeight = 9.0,
                Type = "Export",
                Status = "Completed",
                Operator = "Ahmed Kaci",
                Shift = "Morning"
            });
            _allRows.Add(new WeighmentReportRow
            {
                TicketNumber = "WB-2026-003",
                Date = new DateTime(2026, 5, 23, 10, 0, 0),
                VehicleNumber = "DXB-T-24680",
                DriverName = "Khalid Ibrahim",
                ContainerNumber = "DPWU7654321",
                Material = "Electronics",
                Supplier = "Tech Imports LLC",
                GrossWeight = 15.0,
                TareWeight = 0.0,
                Type = "Import",
                Status = "Pending",
                Operator = "Ahmed Kaci",
                Shift = "Morning"
            });
            _allRows.Add(new WeighmentReportRow
            {
                TicketNumber = "WB-2026-004",
                Date = new DateTime(2026, 5, 23, 15, 20, 0),
                VehicleNumber = "31-SET-1100",
                DriverName = "Rashid Omar",
                ContainerNumber = "TCKU3344556",
                Material = "Automotive Parts",
                Supplier = "Parts Trading LLC",
                GrossWeight = 26.0,
                TareWeight = 7.5,
                Type = "Import",
                Status = "Completed",
                Operator = "Sara Benali",
                Shift = "Afternoon"
            });
            _allRows.Add(new WeighmentReportRow
            {
                TicketNumber = "WB-2026-005",
                Date = new DateTime(2026, 5, 23, 23, 45, 0),
                VehicleNumber = "07-ALG-9934",
                DriverName = "Abdullah Fahad",
                ContainerNumber = "HLXU5566778",
                Material = "Textiles",
                Supplier = "Fabric World DMCC",
                GrossWeight = 19.0,
                TareWeight = 6.8,
                Type = "Export",
                Status = "Completed",
                Operator = "Karim Oussad",
                Shift = "Night"
            });
        }

        // ── Filter + recalculate KPIs ─────────────────────────
        private void ApplyFilter()
        {
            var q = SearchText.Trim().ToLower();

            var filtered = _allRows.AsEnumerable();

            // Status filter
            if (StatusFilter != "All")
                filtered = filtered.Where(r => r.Status == StatusFilter);

            // Search
            if (!string.IsNullOrEmpty(q))
                filtered = filtered.Where(r =>
                    r.TicketNumber.ToLower().Contains(q) ||
                    r.VehicleNumber.ToLower().Contains(q) ||
                    r.DriverName.ToLower().Contains(q) ||
                    r.ContainerNumber.ToLower().Contains(q) ||
                    r.Material.ToLower().Contains(q));

            var list = filtered.ToList();

            FilteredRows.Clear();
            foreach (var r in list) FilteredRows.Add(r);

            // Recalculate KPIs
            TotalWeighments = list.Count;
            CompletedCount = list.Count(r => r.Status == "Completed");
            PendingCount = list.Count(r => r.Status == "Pending");
            CancelledCount = list.Count(r => r.Status == "Cancelled");
            TotalGrossWeight = Math.Round(list.Sum(r => r.GrossWeight), 2);
            TotalNetWeight = Math.Round(list.Where(r => r.Status == "Completed").Sum(r => r.NetWeight), 2);
            AverageNetWeight = CompletedCount == 0 ? 0
                : Math.Round(TotalNetWeight / CompletedCount, 2);
            ImportCount = list.Count(r => r.Type == "Import");
            ExportCount = list.Count(r => r.Type == "Export");

            OnPropertyChanged(nameof(MaxTypeValue));
        }

        // ── Actions ───────────────────────────────────────────
        private void DownloadReport()
        {
            System.Windows.MessageBox.Show(
                $"Generating {ReportType} — {Period}\n" +
                $"{TotalWeighments} records  |  Net: {TotalNetWeight:F2} MT",
                "Download Report");
        }

        private void PrintTicket(WeighmentReportRow? row)
        {
            if (row is null) return;
            System.Windows.MessageBox.Show(
                $"Printing weighbridge ticket {row.TicketNumber}\n" +
                $"Vehicle : {row.VehicleNumber}\n" +
                $"Net     : {row.NetWeight:F2} MT",
                "Print Ticket");
        }

        private void ExportCsv()
        {
            System.Windows.MessageBox.Show(
                $"Exporting {FilteredRows.Count} records to CSV…",
                "Export CSV");
        }
    }

    // ── Row model ─────────────────────────────────────────────
    public class WeighmentReportRow
    {
        public string TicketNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string VehicleNumber { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public string ContainerNumber { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public double GrossWeight { get; set; }
        public double TareWeight { get; set; }
        public double NetWeight => GrossWeight - TareWeight;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;

        // Display helpers
        public string StatusBadgeColor => Status switch
        {
            "Completed" => "#3B6D11",
            "Pending" => "#EF9F27",
            "Cancelled" => "#E8001D",
            _ => "#8A8E9B"
        };
        public string StatusBgColor => Status switch
        {
            "Completed" => "#F1FCE9",   // light green tint
            "Pending" => "#FDF4E7",   // light amber tint
            "Cancelled" => "#FFE6E8",   // light red tint
            _ => "#F4F4F4"
        };

    }

    // ── Shift summary model ───────────────────────────────────
    public class ShiftSummary
    {
        public string ShiftName { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public int OpsCount { get; set; }
        public double TotalWeight { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;

        public string ShiftColor => ShiftName switch
        {
            "Morning" => "#3E3C90",
           // "Afternoon" => "#8CA7D2",
            //"Night" => "#282725",
            "Afternoon" => "#3E3C90",
            "Night" => "#3E3C90",
            _ => "#8A8E9B"
        };
        public string ShiftBgColor => ShiftName switch
        {
            "Morning" => "#EDEDF7",   // warm cream
            "Afternoon" => "#EDF1F8",   // light red tint
            "Night" => "#F3F2F2",   // light navy tint
            _ => "#F4F4F4"
        };
    }
}

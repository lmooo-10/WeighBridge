using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using WeighBridge.Models;
using WeighBridge.Repositories.Interfaces;
using WeighBridge.Services.Interfaces;
using WeighBridge.ViewModels.Base;

namespace WeighBridge.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IWeighmentRepository _repo;
        private readonly IZodiacService _zodiac;
        private readonly ITOSRepository _tos;
        private readonly DispatcherTimer _timer;

        // ── Current user (for Welcome card bindings) ──────────
        public UserModel CurrentUser { get; }

        // ── KPI Cards ─────────────────────────────────────────
        private int _todaysOps;
        private int _importCount;
        private double _todaysDelta;
        private double _importDelta;
        private double _uptime;
        private int _vesselsImport;

        public int TodaysOps { get => _todaysOps; set => SetProperty(ref _todaysOps, value); }
        public int ImportCount { get => _importCount; set => SetProperty(ref _importCount, value); }
        public double TodaysDelta { get => _todaysDelta; set => SetProperty(ref _todaysDelta, value); }
        public double ImportDelta { get => _importDelta; set => SetProperty(ref _importDelta, value); }
        public double Uptime { get => _uptime; set => SetProperty(ref _uptime, value); }
        public int VesselsImport { get => _vesselsImport; set => SetProperty(ref _vesselsImport, value); }

        // ── Donut 1 : Import vs Export ────────────────────────
        private int _importTEU;
        private int _exportTEU;
        private int _totalTEU;
        private double _importPct;
        private double _exportPct;

        public int ImportTEU { get => _importTEU; set => SetProperty(ref _importTEU, value); }
        public int ExportTEU { get => _exportTEU; set => SetProperty(ref _exportTEU, value); }
        public int TotalTEU { get => _totalTEU; set => SetProperty(ref _totalTEU, value); }
        public double ImportPct { get => _importPct; set => SetProperty(ref _importPct, value); }
        public double ExportPct { get => _exportPct; set => SetProperty(ref _exportPct, value); }

        // ── Bar chart ─────────────────────────────────────────
        private int _std20Import; private int _std20Export;
        private int _std40Import; private int _std40Export;
        private int _reeferImport; private int _reeferExport;
        private int _dgrImport; private int _dgrExport;

        public int Std20Import { get => _std20Import; set => SetProperty(ref _std20Import, value); }
        public int Std20Export { get => _std20Export; set => SetProperty(ref _std20Export, value); }
        public int Std40Import { get => _std40Import; set => SetProperty(ref _std40Import, value); }
        public int Std40Export { get => _std40Export; set => SetProperty(ref _std40Export, value); }
        public int ReeferImport { get => _reeferImport; set => SetProperty(ref _reeferImport, value); }
        public int ReeferExport { get => _reeferExport; set => SetProperty(ref _reeferExport, value); }
        public int DgrImport { get => _dgrImport; set => SetProperty(ref _dgrImport, value); }
        public int DgrExport { get => _dgrExport; set => SetProperty(ref _dgrExport, value); }

        // ── Summary totals ────────────────────────────────────
        public int Std20Total => Std20Import + Std20Export;
        public int Std40Total => Std40Import + Std40Export;
        public int ReeferTotal => ReeferImport + ReeferExport;
        public int DgrTotal => DgrImport + DgrExport;

        // ── Max bar value (for scaling all bars) ──────────────
        public int MaxBarValue => Math.Max(1,
            Math.Max(
                Math.Max(Std20Import, Std20Export),
                Math.Max(
                    Math.Max(Std40Import, Std40Export),
                    Math.Max(
                        Math.Max(ReeferImport, ReeferExport),
                        Math.Max(DgrImport, DgrExport)))));

        // ── Donut 2 percentages (computed) ────────────────────
        private int _donut2Total => Std20Total + Std40Total + ReeferTotal + DgrTotal;

        public int Std20Pct => _donut2Total == 0 ? 0 : (int)Math.Round(Std20Total / (double)_donut2Total * 100);
        public int Std40Pct => _donut2Total == 0 ? 0 : (int)Math.Round(Std40Total / (double)_donut2Total * 100);
        public int ReeferPct => _donut2Total == 0 ? 0 : (int)Math.Round(ReeferTotal / (double)_donut2Total * 100);
        public int DgrPct => _donut2Total == 0 ? 0 : (int)Math.Round(DgrTotal / (double)_donut2Total * 100);

        // ── Status ────────────────────────────────────────────
        private bool _isWB01Online;
        private bool _isZodiacConnected;
        private string _tickerMessage = string.Empty;

        public bool IsWB01Online { get => _isWB01Online; set => SetProperty(ref _isWB01Online, value); }
        public bool IsZodiacConnected { get => _isZodiacConnected; set => SetProperty(ref _isZodiacConnected, value); }
        public string TickerMessage { get => _tickerMessage; set => SetProperty(ref _tickerMessage, value); }

        // ── Constructor ───────────────────────────────────────
        public DashboardViewModel(IWeighmentRepository repo,
                                  IZodiacService zodiac,
                                  ITOSRepository tos,
                                  UserModel currentUser)
        {
            _repo = repo;
            _zodiac = zodiac;
            _tos = tos;
            CurrentUser = currentUser;

            _zodiac.WeightChanged += (_, w) =>
                TickerMessage = BuildTicker(w);

            _ = LoadAllAsync();

            _timer = new DispatcherTimer
            { Interval = TimeSpan.FromSeconds(30) };
            _timer.Tick += async (_, _) => await LoadAllAsync();
            _timer.Start();
        }

        private async Task LoadAllAsync()
        {
            await LoadKpisAsync();
            await LoadChartsAsync();
            IsWB01Online = await _zodiac.PingAsync();
            IsZodiacConnected = _zodiac.IsConnected;
            VesselsImport = await _tos.GetImportVesselsCountAsync();
        }

        private async Task LoadKpisAsync()
        {
            int todayOps = await _repo.CountTodayAsync();
            int yesterdayOps = await _repo.CountYesterdayAsync();
            int todayImp = await _repo.CountTodayAsync("Import");
            int yesterdayImp = await _repo.CountYesterdayAsync("Import");

            TodaysOps = todayOps;
            ImportCount = todayImp;
            Uptime = await _repo.GetUptimePctAsync();

            TodaysDelta = yesterdayOps == 0 ? 0
                : Math.Round((todayOps - yesterdayOps) / (double)yesterdayOps * 100, 1);
            ImportDelta = yesterdayImp == 0 ? 0
                : Math.Round((todayImp - yesterdayImp) / (double)yesterdayImp * 100, 1);
        }

        private async Task LoadChartsAsync()
        {

            var stats = await _repo.GetByTypeAndDirectionAsync();

            ImportTEU = stats.Where(s => s.WeighmentType == "Import").Sum(s => s.Count);
            ExportTEU = stats.Where(s => s.WeighmentType == "Export").Sum(s => s.Count);
            TotalTEU = ImportTEU + ExportTEU;
            ImportPct = TotalTEU == 0 ? 0
                : Math.Round(ImportTEU / (double)TotalTEU * 100);
            ExportPct = 100 - ImportPct;

            int Get(string ctn, string dir) => stats
                .FirstOrDefault(s => s.ContainerType == ctn
                                  && s.WeighmentType == dir)?.Count ?? 0;


            Std20Import = Get("IMPU", "Import");
            Std20Export = Get("EXPU", "Export");
            Std40Import = Get("IMPU", "Import");  // adjust keys to match your data
            Std40Export = Get("EXPU", "Export");
            ReeferImport = Get("REEF", "Import");
            ReeferExport = Get("REEF", "Export");
            DgrImport = Get("DGR", "Import");
            DgrExport = Get("DGR", "Export");

            OnPropertyChanged(nameof(Std20Total));
            OnPropertyChanged(nameof(Std40Total));
            OnPropertyChanged(nameof(ReeferTotal));
            OnPropertyChanged(nameof(DgrTotal));
            OnPropertyChanged(nameof(MaxBarValue));
            OnPropertyChanged(nameof(Std20Pct));
            OnPropertyChanged(nameof(Std40Pct));
            OnPropertyChanged(nameof(ReeferPct));
            OnPropertyChanged(nameof(DgrPct));
        }

        private string BuildTicker(double liveWeight) =>
            $"Algiers Terminal  {TodaysOps} ops today  ·  " +
            $"WB-01 Weight {liveWeight * 1000:N0} kg — STABLE  ·  " +
            $"Import {ImportCount} containers  ·  " +
            $"Uptime {Uptime:F1}%  ·  " +
            $"{VesselsImport} Vessels Import  ·  ";
    }
}
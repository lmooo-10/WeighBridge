using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WeighBridge.Models;
using WeighBridge.Repositories.Interfaces;
using WeighBridge.Services.Interfaces;
using WeighBridge.ViewModels.Base;
using WeighBridge.Views.UserControls;
using WeighBridge.Views.Windows;

namespace WeighBridge.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // ── Services ──────────────────────────────────────────
        private readonly IWeighmentRepository _repo;
        private readonly IZodiacService _zodiac;
        private readonly ITOSRepository _tos;

        // ── Current user ──────────────────────────────────────
        public UserModel CurrentUser { get; }

        // ── Role flags (computed once) ────────────────────────
        public bool IsAdmin => CurrentUser.Role == UserRole.Administrateur;
        public bool IsCommercial => CurrentUser.Role == UserRole.Commercial;
        public bool IsAgent => CurrentUser.Role == UserRole.Agent;

        // ── Role-based Sidebar Visibility ─────────────────────
        public Visibility ShowConsole => V(IsAdmin || IsAgent);
        public Visibility ShowReports => V(IsAdmin || IsCommercial);
        public Visibility ShowVGM => V(IsAdmin || IsAgent);
        public Visibility ShowExports => V(IsAdmin || IsCommercial);
        public Visibility ShowTerminalNetwork => V(IsAdmin);
        public Visibility ShowSettings => V(IsAdmin);
        private static Visibility V(bool show) =>
            show ? Visibility.Visible : Visibility.Collapsed;

        // ── Active page ───────────────────────────────────────
        private UserControl _activePage = null!;
        public UserControl ActivePage
        {
            get => _activePage;
            set => SetProperty(ref _activePage, value);
        }

        // ── NavBar title ──────────────────────────────────────
        private string _pageTitle = "GLOBAL OPERATIONS";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        // ── Active menu key (sidebar highlight) ───────────────
        private string _activeMenu = "dashboard";
        public string ActiveMenu
        {
            get => _activeMenu;
            set => SetProperty(ref _activeMenu, value);
        }

        // ── Clock ─────────────────────────────────────────────
        private string _currentTime = DateTime.Now.ToString("HH:mm:ss");
        private string _currentDate = DateTime.Now.ToString("ddd. dd MMM yyyy");
        public string CurrentTime { get => _currentTime; set => SetProperty(ref _currentTime, value); }
        public string CurrentDate { get => _currentDate; set => SetProperty(ref _currentDate, value); }

        // ── Commands ──────────────────────────────────────────
        public RelayCommand NavigateCommand { get; }
        public RelayCommand LogoutCommand { get; }

        // ── Page cache ────────────────────────────────────────
        private readonly Dictionary<string, UserControl> _pageCache = new();

        // ── Constructor ───────────────────────────────────────
        public MainViewModel(UserModel user,
                             IWeighmentRepository repo,
                             IZodiacService zodiac,
                             ITOSRepository tos)
        {
            CurrentUser = user;

            _repo = repo;
            _zodiac = zodiac;
            _tos = tos;

            NavigateCommand = new RelayCommand(ExecuteNavigate);
            LogoutCommand = new RelayCommand(ExecuteLogout);
            _activePage = GetOrCreatePage("dashboard");

            var timer = new System.Windows.Threading.DispatcherTimer
            { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (_, _) =>
            {
                CurrentTime = DateTime.Now.ToString("HH:mm:ss");
                CurrentDate = DateTime.Now.ToString("ddd. dd MMM yyyy");
            };
            timer.Start();
        }

        // ── Navigation ────────────────────────────────────────
        private void ExecuteNavigate(object? param)
        {
            if (param is not string key) return;
            ActiveMenu = key;
            PageTitle = KeyToTitle(key);
            ActivePage = GetOrCreatePage(key);
        }

        // ═══════════════════════════════════════════════════════
        //  THE FIX:
        //  Each page gets its own dedicated ViewModel as DataContext.
        //  Only DashboardControl keeps MainViewModel (it binds to
        //  CurrentUser + role flags that live here).
        //  Every other page gets the matching dedicated ViewModel.
        //  This makes ALL static sample data visible immediately,
        //  and the architecture is ready to swap in real services later.
        // ═══════════════════════════════════════════════════════
        private UserControl GetOrCreatePage(string key)
        {
            if (_pageCache.TryGetValue(key, out var cached)) return cached;

            UserControl page;

            switch (key)
            {
                case "dashboard":
                    page = new DashboardControl();
                    // Dashboard binds to CurrentUser/role flags → MainViewModel
                    page.DataContext = new DashboardViewModel(_repo, _zodiac, _tos, CurrentUser); ;
                    break;

                case "console":
                    page = new WeighingConsoleControl();
                    // WeighingConsoleControl binds to: LiveWeight, Stage, Status,
                    // WeighmentTypes, VehicleNumber, DriverName, ContainerNumber,
                    // Material, Supplier, GrossWeight, TareWeight, NetWeight,
                    // StartWeighingCommand, CompleteWeighmentCommand
                    page.DataContext = new WeighingConsoleViewModel();
                    break;

                case "history":
                    page = new WeighmentHistoryControl();
                    // WeighmentHistoryControl binds to: FilteredRecords,
                    // SearchText, ExportCsvCommand
                    page.DataContext = new WeighmentHistoryViewModel();
                    break;

                case "reports":
                    page = new ReportsControl();
                    // ReportsControl binds to: ReportType, Period, ReportTypes,
                    // Periods, TotalWeighments, TotalWeight, AverageWeight,
                    // Pending, DownloadReportCommand
                    page.DataContext = new ReportsViewModel();
                    break;

                case "vgm":
                    page = new VGMControl();
                    // VGMControl binds to: TotalVGMIssued, TodaysVGM,
                    // Method1Count, Method2Count, FilteredCertificates,
                    // SearchText, IssueCertificateCommand
                    page.DataContext = new VGMViewModel();
                    break;

                case "exports":
                    page = new ExportsControl();
                    // ExportsControl binds to: ExportType, DateRange, Format,
                    // ExportTypes, DateRanges, Formats, WeighmentsAvailable,
                    // VGMCertificates, TotalWeight, ColXxx checkboxes,
                    // ExportDataCommand
                    page.DataContext = new ExportsViewModel();
                    break;

                default:
                    page = new DashboardControl();
                    page.DataContext = this ;
                    break;
            }

            _pageCache[key] = page;
            return page;
        }

        private static string KeyToTitle(string key) => key switch
        {
            "dashboard" => "GLOBAL OPERATIONS",
            "console" => "WEIGHING CONSOLE",
            "history" => "WEIGHMENT HISTORY",
            "reports" => "REPORTS & ANALYTICS",
            "vgm" => "VGM MANAGEMENT",
            "exports" => "DATA EXPORTS",
            _ => "GLOBAL OPERATIONS"
        };

        // ── Logout ────────────────────────────────────────────
        private void ExecuteLogout(object? _)
        {
            var login = new LoginWindow();
            System.Windows.Application.Current.MainWindow = login;
            login.Show();
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().ToList())
                if (w is MainWindow mw) { mw.Close(); break; }
        }
    }
}

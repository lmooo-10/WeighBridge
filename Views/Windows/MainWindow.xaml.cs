using System.Windows;
using WeighBridge.Models;
using WeighBridge.Repositories.Interfaces;
using WeighBridge.Services.Interfaces;
using WeighBridge.ViewModels;
using WeighBridge.Views.UserControls;

namespace WeighBridge.Views.Windows
{

    public partial class MainWindow : Window
    {

        private readonly MainViewModel _vm;

        public MainWindow(UserModel user,
                  IWeighmentRepository repo,
                  IZodiacService zodiac,
                  ITOSRepository tos)
        {
            InitializeComponent();

            // ── Create ViewModel ─────────────────────────────
            _vm          = new MainViewModel(user, repo, zodiac, tos);
            DataContext  = _vm;

            // ── Inject NavBar (Row 0, Col 1) ─────────────────
            var navBar        = new NavBarControl();
            navBar.DataContext = _vm;
            NavBarHost.Content = navBar;

            // ── Inject SideBar (Row 1, Col 0) ────────────────
            var sideBar        = new SideBarControl();
            sideBar.DataContext = _vm;
            SideBarHost.Content = sideBar;

            // ── Bind active page to PageHost ─────────────────
            // When _vm.ActivePage changes → PageHost updates
            _vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(_vm.ActivePage))
                    PageHost.Content = _vm.ActivePage;
            };

            // Set initial page
            PageHost.Content = _vm.ActivePage;
        }
    }
}

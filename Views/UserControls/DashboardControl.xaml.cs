using System.Windows.Controls;
namespace WeighBridge.Views.UserControls
{
    // DataContext is set by MainViewModel.GetOrCreatePage()
    // which passes itself → all bindings resolve to MainViewModel
    public partial class DashboardControl : UserControl
    {
        public DashboardControl() => InitializeComponent();
    }
}

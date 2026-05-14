using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeighBridge.ViewModels;

namespace WeighBridge.Views.UserControls
{
    public partial class NavBarControl : UserControl
    {
        // Each NavBar owns one NotificationViewModel instance
        private readonly NotificationViewModel _notifVm = new();

        public NavBarControl()
        {
            InitializeComponent();

            // Give the popup card its own ViewModel
            NotifControl.DataContext = _notifVm;

            // Keep the badge in sync with unread count
            _notifVm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(NotificationViewModel.BadgeText)
                 || e.PropertyName == nameof(NotificationViewModel.HasUnread)
                 || e.PropertyName == nameof(NotificationViewModel.UnreadCount))
                {
                    TbBadge.Text = _notifVm.BadgeText;
                    BadgeBorder.Visibility = _notifVm.HasUnread
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
            };

            // Initialise badge text immediately
            TbBadge.Text = _notifVm.BadgeText;
            BadgeBorder.Visibility = _notifVm.HasUnread
                ? Visibility.Visible
                : Visibility.Collapsed;

            // Close popup when it loses focus (StaysOpen=False handles most cases,
            // but we also close it when the ViewModel requests it)
            _notifVm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(NotificationViewModel.IsOpen))
                    NotificationPopup.IsOpen = _notifVm.IsOpen;
            };
        }

        // Bell click → toggle popup
        private void BellBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _notifVm.IsOpen = !_notifVm.IsOpen;
            NotificationPopup.IsOpen = _notifVm.IsOpen;
            e.Handled = true;           // don't bubble and immediately close
        }
    }
}
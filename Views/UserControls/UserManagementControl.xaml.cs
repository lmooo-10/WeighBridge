using System.Windows.Controls;
using WeighBridge.ViewModels;

namespace WeighBridge.Views.UserControls
{
    public partial class UserManagementControl : UserControl
    {
        public UserManagementControl()
        {
            InitializeComponent();

            PbPassword.PasswordChanged += (_, _) =>
            {
                if (DataContext is UserManagementViewModel vm)
                    vm.FormPassword = PbPassword.Password;
            };

            DataContextChanged += (_, _) =>
            {
                if (DataContext is UserManagementViewModel vm)
                    vm.RequestClearPassword += () => PbPassword.Clear();
            };
        }
    }
}
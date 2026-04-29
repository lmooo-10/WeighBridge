using System.Windows;
using System.Windows.Controls.Primitives;
using WeighBridge.Models;

namespace WeighBridge.Views.Windows
{
    public partial class LoginWindow : Window
    {
        private static readonly Dictionary<string,(string Id,string Pass,string Hint)>
            _creds = new()
            {
                ["Agent"]          = ("operator",   "operator123",   "operator / operator123"),
                ["Commercial"]     = ("commercial", "commercial123", "commercial / commercial123"),
                ["Administrateur"] = ("admin",      "admin123",      "admin / admin123"),
            };

        private string _selectedRole = "Agent";

        public LoginWindow()
        {
            InitializeComponent();
            var timer = new System.Windows.Threading.DispatcherTimer
                { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (_, _) => UpdateClock();
            UpdateClock();
            timer.Start();
        }

        private void UpdateClock()
        {
            TbTime.Text = DateTime.Now.ToString("HH:mm:ss");
            TbDate.Text = DateTime.Now.ToString("dd MMM yyyy").ToUpper();
        }

        private void RoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleButton clicked) return;
            BtnAgent.IsChecked      = false;
            BtnCommercial.IsChecked = false;
            BtnAdmin.IsChecked      = false;
            clicked.IsChecked       = true;
            _selectedRole = clicked.Tag?.ToString() ?? "Agent";
            if (_creds.TryGetValue(_selectedRole, out var c))
                TbDemoHint.Text = c.Hint;
            TbError.Visibility = Visibility.Collapsed;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            TbError.Visibility = Visibility.Collapsed;
            var id   = TbOperatorId.Text.Trim();
            var pass = PbPassword.Password;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
            { ShowError("Please enter Operator ID and Password."); return; }

            if (!_creds.TryGetValue(_selectedRole, out var expected))
            { ShowError("Unknown role."); return; }

            if (id != expected.Id || pass != expected.Pass)
            { ShowError($"Wrong credentials for '{_selectedRole}'.\nDemo: {expected.Hint}"); return; }

            var user = new UserModel
            {
                OperatorId    = id,
                FullName      = GetFullName(id),
                Initials      = GetInitials(id),
                Role          = ParseRole(_selectedRole),
                Shift         = GetShift(),
                WeighbridgeId = "WB-01"
            };

            // ═══════════════════════════════════════════════════
            // THE FIX:
            // ShutdownMode = OnMainWindowClose
            // → app only shuts down when the MAIN window closes
            // → LoginWindow closing does NOT kill the app
            //
            // Step 1: create MainWindow
            // Step 2: tell WPF this IS the MainWindow
            // Step 3: show it
            // Step 4: close login (safe — MainWindow is alive)
            // ═══════════════════════════════════════════════════
            var main = new MainWindow(user);
            Application.Current.MainWindow = main;   // ← KEY LINE
            main.Show();
            this.Close();
        }

        private void ShowError(string msg)
        {
            TbError.Text       = msg;
            TbError.Visibility = Visibility.Visible;
        }

        private Shift GetShift() => CbShift.SelectedIndex switch
        {
            1 => Shift.Afternoon,
            2 => Shift.Night,
            _ => Shift.Morning
        };

        private static string GetFullName(string id) => id switch
        {
            "admin"      => "Karim Oussad",
            "commercial" => "Sara Benali",
            _            => "Ahmed Kaci"
        };

        private static string GetInitials(string id) => id switch
        {
            "admin" => "KO", "commercial" => "SB", _ => "AK"
        };

        private static UserRole ParseRole(string role) => role switch
        {
            "Commercial"     => UserRole.Commercial,
            "Administrateur" => UserRole.Administrateur,
            _                => UserRole.Agent
        };
    }
}

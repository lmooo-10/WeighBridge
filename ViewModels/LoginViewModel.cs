using WeighBridge.Models;
using WeighBridge.ViewModels.Base;
using WeighBridge.Views.Windows;

namespace WeighBridge.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        // ── Fields ──────────────────────────────────────────────
        private string   _operatorId  = string.Empty;
        private string   _password    = string.Empty;
        private UserRole _selectedRole = UserRole.Agent;
        private Shift    _selectedShift = Shift.Morning;
        private string?  _errorMessage;
        private bool     _isBusy;

        // ── Properties ──────────────────────────────────────────
        public string OperatorId
        {
            get => _operatorId;
            set => SetProperty(ref _operatorId, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public UserRole SelectedRole
        {
            get => _selectedRole;
            set => SetProperty(ref _selectedRole, value);
        }

        public Shift SelectedShift
        {
            get => _selectedShift;
            set => SetProperty(ref _selectedShift, value);
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        // Shift display items for ComboBox
        public List<ShiftItem> Shifts { get; } = new()
        {
            new ShiftItem { Value = Shift.Morning,   Display = "Morning   (06:00 - 14:00)" },
            new ShiftItem { Value = Shift.Afternoon, Display = "Afternoon (14:00 - 22:00)" },
            new ShiftItem { Value = Shift.Night,     Display = "Night     (22:00 - 06:00)" }
        };

        private ShiftItem _selectedShiftItem = null!;
        public ShiftItem SelectedShiftItem
        {
            get => _selectedShiftItem;
            set
            {
                SetProperty(ref _selectedShiftItem, value);
                if (value != null) SelectedShift = value.Value;
            }
        }

        // ── Commands ────────────────────────────────────────────
        public RelayCommand LoginCommand { get; }

        // ── Constructor ─────────────────────────────────────────
        public LoginViewModel()
        {
            _selectedShiftItem = Shifts[0];
            LoginCommand = new RelayCommand(ExecuteLogin, _ => !IsBusy);
        }

        // ── Logic ───────────────────────────────────────────────
        private void ExecuteLogin(object? _)
        {
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(OperatorId) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter Operator ID and Password.";
                return;
            }

            // Demo credentials
            bool valid = (OperatorId == "operator"      && Password == "operator123")  ||
                         (OperatorId == "commercial"    && Password == "commercial123") ||
                         (OperatorId == "admin"         && Password == "admin123");

            if (!valid)
            {
                ErrorMessage = "Invalid credentials. Try: operator / operator123";
                return;
            }

            var user = new UserModel
            {
                OperatorId = OperatorId,
                FullName   = BuildFullName(OperatorId),
                Initials   = BuildInitials(OperatorId),
                Role       = SelectedRole,
                Shift      = SelectedShift
            };

            // Open MainWindow, close login
            var main = new MainWindow(user);
            main.Show();

            // Close LoginWindow (caller)
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
                if (w is LoginWindow lw) { lw.Close(); break; }
        }

        private static string BuildFullName(string id) => id switch
        {
            "admin"      => "Karim Oussad",
            "commercial" => "Sara Benali",
            _            => "Operator User"
        };

        private static string BuildInitials(string id) => id switch
        {
            "admin"      => "KO",
            "commercial" => "SB",
            _            => "OP"
        };
    }

    public class ShiftItem
    {
        public Shift  Value   { get; set; }
        public string Display { get; set; } = string.Empty;
    }
}

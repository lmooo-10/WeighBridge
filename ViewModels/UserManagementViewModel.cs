using WeighBridge.Models;
using WeighBridge.ViewModels.Base;

namespace WeighBridge.ViewModels
{
    public class UserManagementViewModel : BaseViewModel
    {
        // ── User list ────────────────────────────────────────
        private List<UserModel> _users = new()
        {
            new UserModel { OperatorId = "operator",   FullName = "Ahmed Kaci",   Initials = "AK", Role = UserRole.Agent,          IsActive = true  },
            new UserModel { OperatorId = "commercial", FullName = "Sara Benali",  Initials = "SB", Role = UserRole.Commercial,     IsActive = true  },
            new UserModel { OperatorId = "admin",      FullName = "Karim Oussad", Initials = "KO", Role = UserRole.Administrateur, IsActive = true  },
        };

        public List<UserModel> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        // ── Selected user ─────────────────────────────────────
        private UserModel? _selectedUser;
        public UserModel? SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);
                if (value != null)
                {
                    LoadFormFrom(value);
                    IsFormVisible = true;
                }
            }
        }

        // ── Form fields ───────────────────────────────────────
        private string _formOperatorId = string.Empty;
        private string _formFullName = string.Empty;
        private string _formPassword = string.Empty;
        private UserRole _formRole = UserRole.Agent;
        private bool _formIsActive = true;
        private string? _message;
        private bool _hasMessage;


        public string FormOperatorId { get => _formOperatorId; set => SetProperty(ref _formOperatorId, value); }
        public string FormFullName { get => _formFullName; set => SetProperty(ref _formFullName, value); }
        public string FormPassword { get => _formPassword; set => SetProperty(ref _formPassword, value); }
        public UserRole FormRole { get => _formRole; set => SetProperty(ref _formRole, value); }
        public bool FormIsActive { get => _formIsActive; set => SetProperty(ref _formIsActive, value); }
        public string? Message { get => _message; set { SetProperty(ref _message, value); OnPropertyChanged(nameof(HasMessage)); } }
        public bool HasMessage => !string.IsNullOrEmpty(_message);
        public int ActiveCount => Users.Count(u => u.IsActive);
        public int AdminCount => Users.Count(u => u.Role == UserRole.Administrateur);
        public event Action? RequestClearPassword;
        private bool _isFormVisible = false;
        public bool IsFormVisible
        {
            get => _isFormVisible;
            set => SetProperty(ref _isFormVisible, value);
        }

        public List<UserRole> Roles { get; } = new()
        {
            UserRole.Agent,
            UserRole.Commercial,
            UserRole.Administrateur
        };

        // ── Commands ──────────────────────────────────────────
        public RelayCommand SaveUserCommand { get; }
        public RelayCommand DeactivateCommand { get; }
        public RelayCommand ResetPasswordCommand { get; }
        public RelayCommand NewUserCommand { get; }

        public UserManagementViewModel()
        {
            SaveUserCommand = new RelayCommand(_ => SaveUser(), _ =>  !string.IsNullOrWhiteSpace(FormFullName)
                                                                   && !string.IsNullOrWhiteSpace(FormOperatorId)
                                                                   && (SelectedUser != null || !string.IsNullOrWhiteSpace(FormPassword)));
            DeactivateCommand = new RelayCommand(_ => ToggleActive(), _ => SelectedUser != null);
            ResetPasswordCommand = new RelayCommand(_ => ResetPassword(), _ => SelectedUser != null);
            // Show form when NEW USER is clicked
            NewUserCommand = new RelayCommand(_ => { ClearForm(); IsFormVisible = true; });
        }

        // ── Logic ─────────────────────────────────────────────
        private void SaveUser()
        {
            if (SelectedUser != null)
            {
                // Edit existing
                SelectedUser.FullName = FormFullName;
                SelectedUser.Role = FormRole;
                SelectedUser.IsActive = FormIsActive;
                Message = $"User '{FormFullName}' updated successfully.";
            }
            else
            {
                // Create new
                var newUser = new UserModel
                {
                    OperatorId = FormOperatorId,
                    FullName = FormFullName,
                    Initials = FormFullName.Length >= 2 ? FormFullName[..2].ToUpper() : "??",
                    Role = FormRole,
                    IsActive = true
                };
                _users = new List<UserModel>(_users) { newUser };
                OnPropertyChanged(nameof(Users));
                Message = $"User '{FormFullName}' created successfully.";
            }
            ClearForm();
            OnPropertyChanged(nameof(ActiveCount));
            OnPropertyChanged(nameof(AdminCount));
        }

        private void ToggleActive()
        {
            if (SelectedUser is null) return;
            SelectedUser.IsActive = !SelectedUser.IsActive;
            Message = $"User '{SelectedUser.FullName}' {(SelectedUser.IsActive ? "activated" : "deactivated")}.";
            OnPropertyChanged(nameof(Users));
            OnPropertyChanged(nameof(ActiveCount));
            OnPropertyChanged(nameof(AdminCount));
        }

        private void ResetPassword()
        {
            if (SelectedUser is null) return;
            // In real app: call API. For now just show message.
            Message = $"Password reset link sent for '{SelectedUser.FullName}'.";
        }

        private void ClearForm()
        {
            SelectedUser = null;
            FormOperatorId = string.Empty;
            FormFullName = string.Empty;
            FormPassword = string.Empty;
            FormRole = UserRole.Agent;
            FormIsActive = true;
            Message = null;
            IsFormVisible = false;
            RequestClearPassword?.Invoke();
        }

        private void LoadFormFrom(UserModel u)
        {
            FormOperatorId = u.OperatorId;
            FormFullName = u.FullName;
            FormRole = u.Role;
            FormIsActive = u.IsActive;
            FormPassword = string.Empty;
        }
    }
}

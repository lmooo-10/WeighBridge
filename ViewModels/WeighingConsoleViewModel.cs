using WeighBridge.Models;
using WeighBridge.ViewModels.Base;

namespace WeighBridge.ViewModels
{
    public class WeighingConsoleViewModel : BaseViewModel
    {
        private double _liveWeight = 0.0;
        private string _stage = "GROSS WEIGHT";
        private string _status = "Idle";
        private bool _isWeighing = false;

        // Form fields
        private string _weighmentType = "Import";
        private string _vehicleNumber = string.Empty;
        private string _driverName = string.Empty;
        private string _containerNumber = string.Empty;
        private string _material = string.Empty;
        private string _supplier = string.Empty;

        private double _grossWeight = 0.0;
        private double _tareWeight = 0.0;
        private double _netWeight = 0.0;
        private int _singleModeStep = 0;

        // ── Authorization fields ──────────────────────────────────────────────────
        private bool _isAuthorized = false;
        private bool _isCheckingAuth = false;
        private bool _isAuthStatusVisible = false;
        private string _authStatus = string.Empty;
        private string _clientName = string.Empty;
        private string _companyName = string.Empty;
        private string _bookingNumber = string.Empty;
        private double _zodiacTareWeight = 0.0;

        public bool IsAuthorized { get => _isAuthorized; set => SetProperty(ref _isAuthorized, value); }
        public bool IsCheckingAuth { get => _isCheckingAuth; set => SetProperty(ref _isCheckingAuth, value); }
        public bool IsAuthStatusVisible { get => _isAuthStatusVisible; set => SetProperty(ref _isAuthStatusVisible, value); }
        public string AuthStatus { get => _authStatus; set => SetProperty(ref _authStatus, value); }
        public string ClientName { get => _clientName; set => SetProperty(ref _clientName, value); }
        public string CompanyName { get => _companyName; set => SetProperty(ref _companyName, value); }
        public string BookingNumber { get => _bookingNumber; set => SetProperty(ref _bookingNumber, value); }
        public double ZodiacTareWeight { get => _zodiacTareWeight; set => SetProperty(ref _zodiacTareWeight, value); }

        public RelayCommand CheckAuthorizationCommand { get; }

        private void CheckAuthorization()
        {
            IsCheckingAuth = true;
            IsAuthStatusVisible = false;
            IsAuthorized = false;

            var timer = new System.Windows.Threading.DispatcherTimer
            { Interval = TimeSpan.FromMilliseconds(800) };

            timer.Tick += (_, _) =>
            {
                timer.Stop();
                IsCheckingAuth = false;

                // Mock — replace with real Zodiac call later
                bool authorized = !string.IsNullOrWhiteSpace(ContainerNumber);

                IsAuthorized = authorized;
                IsAuthStatusVisible = true;

                if (authorized)
                {
                    AuthStatus = $"Container {ContainerNumber} authorized";
                    DriverName = "Ahmed Kaci";
                    ClientName = "CEVITAL SPA";
                    CompanyName = "CEVITAL SPA";
                    BookingNumber = "BK-2025-0042";
                    ZodiacTareWeight = 4200;
                }
                else
                {
                    AuthStatus = $"Container {ContainerNumber} not found";
                    DriverName = string.Empty;
                    ClientName = string.Empty;
                    CompanyName = string.Empty;
                }

                CheckAuthorizationCommand.RaiseCanExecuteChanged();
            };

            timer.Start();
        }

        private void ResetAuthorization()
        {
            IsAuthorized = false;
            IsAuthStatusVisible = false;
            AuthStatus = string.Empty;
            DriverName = string.Empty;
            ClientName = string.Empty;
            CompanyName = string.Empty;
            BookingNumber = string.Empty;
            ZodiacTareWeight = 0;
        }
        //public double NetWeight { get => _netWeight; set => SetProperty(ref _netWeight, value); }

        // ── Dual-20ft fields ──────────────────────────────────────────────────
        private bool _isDual20Mode = false;
        private string _containerNumber2 = string.Empty;
        private double _grossWeightN1 = 0.0;   // Pesée 1  : camion + ctn1 + ctn2
        private double _tareN1 = 0.0; //_intermediateWeightN1 = 0.0;   // Tare N1  : camion + ctn2  (= brut ctn2)
        private double _tareN2 = 0.0; //_grossWeightN2 = 0.0;   // Pesée 2  : camion + ctn2  (capturé ci-dessus)
        //private double _tareWeightN2 = 0.0;   // Tare N2  : camion vide
        private int _dual20Step = 0;     // 0=idle 1=N1gross 2=N1tare/N2gross 3=N2tare 4=done

        private double _netWeightCtn1 = 0.0;
        private double _netWeightCtn2 = 0.0;

        public double NetWeightCtn1 { get => _netWeightCtn1; set => SetProperty(ref _netWeightCtn1, value); }
        public double NetWeightCtn2 { get => _netWeightCtn2; set => SetProperty(ref _netWeightCtn2, value); }

        public double NetWeight { get => _netWeight; set => SetProperty(ref _netWeight, value); }

        public double LiveWeight { get => _liveWeight; set => SetProperty(ref _liveWeight, value); }
        public string Stage { get => _stage; set => SetProperty(ref _stage, value); }
        public string Status { get => _status; set => SetProperty(ref _status, value); }
        public bool IsWeighing { get => _isWeighing; set => SetProperty(ref _isWeighing, value); }
        //public string WeighmentType { get => _weighmentType; set => SetProperty(ref _weighmentType, value); }
        public string VehicleNumber { get => _vehicleNumber; set => SetProperty(ref _vehicleNumber, value); }
        public string DriverName { get => _driverName; set => SetProperty(ref _driverName, value); }
        public string ContainerNumber { get => _containerNumber; set => SetProperty(ref _containerNumber, value); }
        public string Material { get => _material; set => SetProperty(ref _material, value); }
        public string Supplier { get => _supplier; set => SetProperty(ref _supplier, value); }

        public double GrossWeight { get => _grossWeight; set { SetProperty(ref _grossWeight, value); OnPropertyChanged(nameof(NetWeight)); } }
        public double TareWeight { get => _tareWeight; set { SetProperty(ref _tareWeight, value); OnPropertyChanged(nameof(NetWeight)); } }
        //public double NetWeight => GrossWeight - TareWeight;

        // ── Dual-20ft public surface ──────────────────────────────────────────
        public bool IsDual20Mode { get => _isDual20Mode; set { SetProperty(ref _isDual20Mode, value); OnPropertyChanged(nameof(IsSingleMode)); RefreshDual20Commands(); } }
        public bool IsSingleMode => !IsDual20Mode;

        public string ContainerNumber2 { get => _containerNumber2; set => SetProperty(ref _containerNumber2, value); }
        public double GrossWeightN1 { get => _grossWeightN1; set { SetProperty(ref _grossWeightN1, value); OnPropertyChanged(nameof(NetWeightCtn1)); } }
        public double TareN1 { get => _tareN1; set { SetProperty(ref _tareN1, value); OnPropertyChanged(nameof(NetWeightCtn1)); OnPropertyChanged(nameof(NetWeightCtn2)); } }
        public double TareN2 { get => _tareN2; set { SetProperty(ref _tareN2, value); OnPropertyChanged(nameof(NetWeightCtn2)); } }

        /// Net ctn1 = (camion+ctn1+ctn2) − (camion+ctn2)public double NetWeightCtn1 => GrossWeightN1 - TareN1;
        /// Net ctn2 = (camion+ctn2)     − (camion vide)public double NetWeightCtn2 => TareN1 - TareN2;

        public int SingleModeStep
        {
            get => _singleModeStep;
            set
            {
                SetProperty(ref _singleModeStep, value);
                OnPropertyChanged(nameof(SingleModeStageLabel)); // ✅ label updates automatically
            }
        }

        public int Dual20Step
        {
            get => _dual20Step;
            set
            {
                SetProperty(ref _dual20Step, value);
                OnPropertyChanged(nameof(Dual20StageLabel)); // ✅ label updates automatically
            }
        }

        public string SingleModeStageLabel => _singleModeStep switch
        {
            1 => "GROSS WEIGHT",// "PESÉE N1 — Camion chargé (CTN1 + CTN2)",
            2 => "TARE ", //"TARE N1 / PESÉE N2 — Camion chargé (CTN2)",
            3 => "COMPLETE",
            _ => "WAITING"
        };

        public string Dual20StageLabel => _dual20Step switch
        {
            1 => "GROSS WEIGHT",// "PESÉE N1 — Camion chargé (CTN1 + CTN2)",
            2 => "TARE N1", //"TARE N1 / PESÉE N2 — Camion chargé (CTN2)",
            3 => "TARE N2", //"TARE N2 — Camion vide",
            4 => "COMPLETE",
            _ => "WAITING"
        };

        public List<string> WeighmentTypes { get; } = new() { "Import", "Export" };

        // ── Commands ──────────────────────────────────────────────────────────
        public RelayCommand StartSingleModeCommand { get; }
        public RelayCommand ConfirmSingleModeCommand { get; }

        public RelayCommand StartDual20Command { get; }   // démarre étape courante
        public RelayCommand ConfirmDual20Command { get; }   // confirme / capture le poids
        public RelayCommand ToggleModeCommand { get; }
        public RelayCommand ResetDual20Command { get; }
        public RelayCommand ResetSingleModeCommand { get; }

        public RelayCommand PrintImportTicketCommand { get; }
        public RelayCommand PrintVGMCommand { get; }

        public WeighingConsoleViewModel()
        {

            StartSingleModeCommand = new RelayCommand(_ => StartSingleModeStep(), _ => !IsWeighing && _singleModeStep < 4 && IsSingleMode);
            ConfirmSingleModeCommand = new RelayCommand(_ => ConfirmSingleModeStep(), _ => IsWeighing && IsSingleMode);

            StartDual20Command = new RelayCommand(_ => StartDual20Step(), _ => IsDual20Mode && !IsWeighing && _dual20Step < 4);
            ConfirmDual20Command = new RelayCommand(_ => ConfirmDual20Step(), _ => IsDual20Mode && IsWeighing);

            ToggleModeCommand = new RelayCommand(p => IsDual20Mode = (string)p == "Dual20");
            ResetDual20Command = new RelayCommand(_ => ResetDual20());
            ResetSingleModeCommand = new RelayCommand(_ => ResetSingleMode());

            PrintImportTicketCommand = new RelayCommand(_ => PrintTicket("Import"), _ => WeighmentType == "Import");
            PrintVGMCommand = new RelayCommand(_ => PrintTicket("Export"), _ => WeighmentType == "Export");

            CheckAuthorizationCommand = new RelayCommand(
    _ => CheckAuthorization(),
    _ => !string.IsNullOrWhiteSpace(ContainerNumber) && !IsCheckingAuth);
        }

        // ── Single-weighment logic (unchanged) ────────────────────────────────
        private void StartSingleModeStep()
        {;
            if (_singleModeStep == 0) SingleModeStep = 1;   // première activation

            IsWeighing = true;
            Status = $"Weigh {SingleModeStageLabel}…";
            OnPropertyChanged(nameof(SingleModeStageLabel));

            SimulateLiveWeight(targetTicks: 20, onComplete: () =>
            {
                // Ne capture pas encore — l'opérateur valide via ConfirmDual20Command
                Status = "Stable — press to Confirm";
            });
        }

        private void ConfirmSingleModeStep()
        {
            IsWeighing = false;

            switch (_singleModeStep)
            {
                case 1:                                     // ── Pesée N1 : brut total
                    GrossWeight = LiveWeight;
                    Stage = "GROSS WEIGHT";   // "TARE N1 / PESÉE N2";
                    Status = "Gross weight recorded — unload CTN1 and return to the scale"; //"CTN1 enregistré — déchargez CTN1, remontez sur le pont";
                    SingleModeStep = 2;
                    break;

                case 2:                                     // ── Tare N1 = brut N2
                    TareWeight = LiveWeight; //IntermediateWeightN1 = LiveWeight;
                    NetWeight = Math.Round(GrossWeight - TareWeight,2);
                    Stage = "COMPLETE";
                    Status = $"net : {NetWeight:F1} t";//Status = $"CTN1 net : {NetWeightCtn1:F2} t  |  CTN2 net : {NetWeightCtn2:F2} t";
                    SingleModeStep = 3;
                    break;
            }

            OnPropertyChanged(nameof(SingleModeStageLabel));
        }

        // ── Dual-20ft logic ───────────────────────────────────────────────────

        /// <summary>Lance la lecture du pont bascule pour l'étape en cours.</summary>
        private void StartDual20Step()
        {
            if (_dual20Step == 0) Dual20Step = 1;   // première activation

            IsWeighing = true;
            Status = $"Weigh {Dual20StageLabel}…";
            OnPropertyChanged(nameof(Dual20StageLabel));

            SimulateLiveWeight(targetTicks: 20, onComplete: () =>
            {
                // Ne capture pas encore — l'opérateur valide via ConfirmDual20Command
                Status = "Stable — press confirm the weight";
            });
        }

        /// <summary>Capture le poids affiché et avance à l'étape suivante.</summary>
        private void ConfirmDual20Step()
        {
            IsWeighing = false;

            switch (_dual20Step)
            {
                case 1:                                     // ── Pesée N1 : brut total
                    GrossWeightN1 = LiveWeight;
                    Stage = "GROSS WEIGHT";   // "TARE N1 / PESÉE N2";
                    Status = "Gross weight recorded — unload CTN1 and return to the scale"; //"CTN1 enregistré — déchargez CTN1, remontez sur le pont";
                    Dual20Step = 2;
                    break;

                case 2:                                     // ── Tare N1 = brut N2
                    TareN1 = LiveWeight; //IntermediateWeightN1 = LiveWeight;
                    NetWeightCtn1 = Math.Round(GrossWeightN1 - TareN1, 2);

                    Stage = "TARE N1"; //"TARE N2";
                    Status = "CTN1 gross recorded — unload CTN2, return empty to the scale"; //"CTN2 brut enregistré — déchargez CTN2, remontez vide";
                    Dual20Step = 3;
                    break;

                case 3:                                     // ── Tare N2 : camion vide
                    TareN2 = LiveWeight;
                    // Consolide les champs hérités pour cohérence avec le mode simple
                    GrossWeight = GrossWeightN1;
                    TareWeight = TareN2;
                    NetWeightCtn2 = Math.Round(TareN1 - TareN2, 2);

                    Stage = "COMPLETE";
                    Status = $"CTN1 net : {NetWeightCtn1:F2} t  |  CTN2 net : {NetWeightCtn2:F2} t";
                    Dual20Step = 4;
                    break;
            }

            OnPropertyChanged(nameof(Dual20StageLabel));
        }

        public void ResetSingleMode()
        {
            SingleModeStep = 0;
            GrossWeight = 0.0;
            TareWeight = 0.0;
            LiveWeight = 0.0;
            NetWeight = 0.0;
            Stage = "GROSS WEIGHT";
            Status = "Idle";
            IsWeighing = false;
          //  RefreshSingleModeCommands();
        }

        /// <summary>Remet à zéro tous les champs dual-20 pour une nouvelle opération.</summary>
        public void ResetDual20()
        {
            Dual20Step = 0;
            GrossWeightN1 = 0.0;
            TareN1 = 0.0;
            TareN2 = 0.0;
            NetWeightCtn1 = 0.0;
            NetWeightCtn2 = 0.0;
            ContainerNumber2 = string.Empty;
            LiveWeight = 0.0;
            Stage = "GROSS WEIGHT";
            Status = "Idle";
            IsWeighing = false;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void SimulateLiveWeight(int targetTicks, Action onComplete)
        {
            var rng = new Random();
            var timer = new System.Windows.Threading.DispatcherTimer
            { Interval = TimeSpan.FromMilliseconds(200) };
            int ticks = 0;
            timer.Tick += (_, _) =>
            {
                LiveWeight = Math.Round(15.0 + rng.NextDouble() * 25.0, 2);
                ticks++;
                if (ticks >= targetTicks) { timer.Stop(); onComplete(); }
            };
            timer.Start();
        }

        private void RefreshSingleModeCommands()
        {
            StartSingleModeCommand.RaiseCanExecuteChanged();
            ConfirmSingleModeCommand.RaiseCanExecuteChanged();
        }

        private void RefreshDual20Commands()
        {
            StartDual20Command.RaiseCanExecuteChanged();
            ConfirmDual20Command.RaiseCanExecuteChanged();
            StartSingleModeCommand.RaiseCanExecuteChanged();
            ConfirmSingleModeCommand.RaiseCanExecuteChanged();
        }

        //──────── Tickets ───────────────────────────────────────────────────────────
        private void PrintTicket(string type)
        {
            // TODO: hook into your actual print/report service
            System.Windows.MessageBox.Show(
                $"Printing {type} ticket for container {ContainerNumber}…",
                $"Print {type} Ticket",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }

        public string WeighmentType
        {
            get => _weighmentType;
            set
            {
                SetProperty(ref _weighmentType, value);
                PrintImportTicketCommand?.RaiseCanExecuteChanged();
                PrintVGMCommand?.RaiseCanExecuteChanged();
            }
        }
    }
}

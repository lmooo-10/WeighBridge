using WeighBridge.Models;
using WeighBridge.ViewModels.Base;

namespace WeighBridge.ViewModels
{
    public class WeighingConsoleViewModel : BaseViewModel
    {
        private double  _liveWeight    = 0.0;
        private string  _stage         = "GROSS WEIGHT";
        private string  _status        = "Idle";
        private bool    _isWeighing    = false;

        // Form fields
        private string _weighmentType  = "Import";
        private string _vehicleNumber  = string.Empty;
        private string _driverName     = string.Empty;
        private string _containerNumber = string.Empty;
        private string _material       = string.Empty;
        private string _supplier       = string.Empty;

        private double _grossWeight    = 0.0;
        private double _tareWeight     = 0.0;

        public double  LiveWeight      { get => _liveWeight;     set => SetProperty(ref _liveWeight,     value); }
        public string  Stage           { get => _stage;          set => SetProperty(ref _stage,          value); }
        public string  Status          { get => _status;         set => SetProperty(ref _status,         value); }
        public bool    IsWeighing      { get => _isWeighing;     set => SetProperty(ref _isWeighing,     value); }
        public string  WeighmentType   { get => _weighmentType;  set => SetProperty(ref _weighmentType,  value); }
        public string  VehicleNumber   { get => _vehicleNumber;  set => SetProperty(ref _vehicleNumber,  value); }
        public string  DriverName      { get => _driverName;     set => SetProperty(ref _driverName,     value); }
        public string  ContainerNumber { get => _containerNumber;set => SetProperty(ref _containerNumber,value); }
        public string  Material        { get => _material;       set => SetProperty(ref _material,       value); }
        public string  Supplier        { get => _supplier;       set => SetProperty(ref _supplier,       value); }
        public double  GrossWeight     { get => _grossWeight;    set { SetProperty(ref _grossWeight, value); OnPropertyChanged(nameof(NetWeight)); } }
        public double  TareWeight      { get => _tareWeight;     set { SetProperty(ref _tareWeight,  value); OnPropertyChanged(nameof(NetWeight)); } }
        public double  NetWeight       => GrossWeight - TareWeight;

        public List<string> WeighmentTypes { get; } = new() { "Import", "Export" };

        public RelayCommand StartWeighingCommand    { get; }
        public RelayCommand CompleteWeighmentCommand { get; }

        public WeighingConsoleViewModel()
        {
            StartWeighingCommand     = new RelayCommand(_ => StartWeighing(),    _ => !IsWeighing);
            CompleteWeighmentCommand = new RelayCommand(_ => CompleteWeighment(), _ => IsWeighing);
        }

        private void StartWeighing()
        {
            IsWeighing = true;
            Status     = "Weighing...";
            // Simulate live weight (in real app: read from serial port / Zodiac)
            var rng    = new Random();
            var timer  = new System.Windows.Threading.DispatcherTimer
                { Interval = TimeSpan.FromMilliseconds(200) };
            int ticks  = 0;
            timer.Tick += (_, _) =>
            {
                LiveWeight = Math.Round(15.0 + rng.NextDouble() * 25.0, 2);
                ticks++;
                if (ticks >= 20) { timer.Stop(); GrossWeight = LiveWeight; Stage = "TARE WEIGHT"; }
            };
            timer.Start();
        }

        private void CompleteWeighment()
        {
            TareWeight = Math.Round(LiveWeight * 0.3, 2);
            Stage      = "COMPLETE";
            Status     = "Done";
            IsWeighing = false;
        }
    }
}

using WeighBridge.Services.Interfaces;

namespace WeighBridge.Services
{
    public class MockZodiacService : IZodiacService
    {
        private readonly System.Windows.Threading.DispatcherTimer _timer;
        private readonly Random _rng = new();

        public bool IsConnected { get; private set; } = true;
        public double CurrentWeight { get; private set; } = 0.0;

        public event EventHandler<double>? WeightChanged;

        public MockZodiacService()
        {
            _timer = new System.Windows.Threading.DispatcherTimer
            { Interval = TimeSpan.FromSeconds(3) };
            _timer.Tick += (_, _) =>
            {
                CurrentWeight = Math.Round(10.0 + _rng.NextDouble() * 30.0, 2);
                WeightChanged?.Invoke(this, CurrentWeight);
            };
            _timer.Start();
        }

        public Task<bool> PingAsync() => Task.FromResult(true);
        public void StartListening(string portName = "COM3", int baudRate = 9600) { }
        public void StopListening() { }
    }
}
// Services/Mock/MockZodiacService.cs
using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using WeighBridge.Services.Interfaces;

namespace WeighBridge.Services.Mock
{
    public class MockZodiacService : IZodiacService
    {
        private readonly DispatcherTimer _timer;
        private readonly Random _rng = new();

        public bool IsConnected { get; private set; } = true;
        public double CurrentWeight { get; private set; } = 28.45;

        public event EventHandler<double>? WeightChanged;

        public MockZodiacService()
        {
            // Simulate weight fluctuating every 2 seconds
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _timer.Tick += (_, _) =>
            {
                // Simulate small fluctuation around a base weight
                CurrentWeight = Math.Round(27.0 + _rng.NextDouble() * 3.0, 2);
                WeightChanged?.Invoke(this, CurrentWeight);
            };
            _timer.Start();
        }

        public Task<bool> PingAsync()
            => Task.FromResult(true);

        public void StartListening(string portName = "COM3", int baudRate = 9600)
        {
            // No real port — already simulating in constructor
            IsConnected = true;
        }

        public void StopListening()
            => _timer.Stop();

        public void Dispose()
            => _timer.Stop();
    }
}
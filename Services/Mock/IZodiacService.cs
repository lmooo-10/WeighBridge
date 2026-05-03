// Services/Mock/MockZodiacService.cs
using System;
using System.Threading.Tasks;

namespace WeighBridge.Services.Interfaces
{
    public interface IZodiacService
    {
        bool IsConnected { get; }
        double CurrentWeight { get; }

        Task<bool> PingAsync();
        void StartListening(string portName = "COM3", int baudRate = 9600);
        void StopListening();

        event EventHandler<double> WeightChanged;
    }
}
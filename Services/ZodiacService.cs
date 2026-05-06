using System;
using System.Threading.Tasks;
using WeighBridge.Services.Interfaces;

namespace WeighBridge.Services
{
    public class ZodiacService : IZodiacService
    {
        // TODO: inject serial port / Modbus client here

        public bool IsConnected { get; private set; } = false;
        public double CurrentWeight { get; private set; } = 0.0;

        public event EventHandler<double>? WeightChanged;

        public Task<bool> PingAsync()
        {
            // TODO: ping real Zodiac device
            throw new NotImplementedException();
        }

        public void StartListening(string portName = "COM3", int baudRate = 9600)
        {
            // TODO: open serial port and start reading weight frames
            throw new NotImplementedException();
        }

        public void StopListening()
        {
            // TODO: close serial port
            throw new NotImplementedException();
        }
    }
}
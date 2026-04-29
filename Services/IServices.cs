using System;
using System.Collections.Generic;
using System.Text;

using WeighBridge.Models;

namespace WeighBridge.Services
{
    // ═══════════════════════════════════════════════════════════
    //  IWeighmentService
    //  Contract between ViewModels and the data layer.
    //  Currently implemented by MockWeighmentService (static data).
    //  Replace with ApiWeighmentService or DbWeighmentService
    //  when backend is ready — zero ViewModel changes required.
    // ═══════════════════════════════════════════════════════════
    public interface IWeighmentService
    {
        Task<List<WeighmentModel>> GetAllWeighmentsAsync();
        Task<WeighmentModel?> GetWeighmentByTicketAsync(string ticketNumber);
        Task<bool> SaveWeighmentAsync(WeighmentModel weighment);
        Task<bool> UpdateWeighmentAsync(WeighmentModel weighment);
        Task<bool> DeleteWeighmentAsync(string ticketNumber);
    }

    // ═══════════════════════════════════════════════════════════
    //  IVGMService
    // ═══════════════════════════════════════════════════════════
    public interface IVGMService
    {
        Task<List<VGMModel>> GetAllCertificatesAsync();
        Task<bool> IssueCertificateAsync(VGMModel certificate);
    }

    // ═══════════════════════════════════════════════════════════
    //  IWeighbridgeHardwareService
    //  Reads live weight from scale (serial port / Modbus / Zodiac).
    //  MockHardwareService returns simulated values.
    // ═══════════════════════════════════════════════════════════
    public interface IWeighbridgeHardwareService
    {
        event Action<double> WeightChanged;
        void StartReading();
        void StopReading();
    }
}


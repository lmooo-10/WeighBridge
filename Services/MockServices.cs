using System;
using System.Collections.Generic;
using System.Text;

using WeighBridge.Models;

namespace WeighBridge.Services
{
    // ═══════════════════════════════════════════════════════════
    //  MockWeighmentService
    //  Returns the same static sample data currently hard-coded
    //  in the ViewModels — but now centralised in ONE place.
    //
    //  To wire to a real backend:
    //    1. Create ApiWeighmentService : IWeighmentService
    //    2. Use HttpClient to call your REST API
    //    3. Register it in ServiceLocator instead of this class
    //    4. ViewModels don't change — they only call the interface
    // ═══════════════════════════════════════════════════════════
    public class MockWeighmentService : IWeighmentService
    {
        private static readonly List<WeighmentModel> _store = new()
        {
            new WeighmentModel
            {
                TicketNumber  = "WB-2026-001",
                DateTime      = new DateTime(2026, 4, 6, 8, 30, 0),
                VehicleNumber = "DXB-T-12345",
                DriverName    = "Ahmed Hassan",
                Material      = "Steel Coils",
                Supplier      = "Gulf Steel Industries",
                GrossWeight   = 28.5,
                TareWeight    = 8.5,
                Type          = WeighmentType.Inbound,
                Status        = WeighmentStatus.Completed,
                OperatorId    = "operator"
            },
            new WeighmentModel
            {
                TicketNumber  = "WB-2026-002",
                DateTime      = new DateTime(2026, 4, 6, 9, 15, 0),
                VehicleNumber = "DXB-T-67890",
                DriverName    = "Mohammed Ali",
                Material      = "Construction Materials",
                Supplier      = "Construction Co.",
                GrossWeight   = 32.0,
                TareWeight    = 9.0,
                Type          = WeighmentType.Outbound,
                Status        = WeighmentStatus.Completed,
                OperatorId    = "operator"
            },
            new WeighmentModel
            {
                TicketNumber  = "WB-2026-003",
                DateTime      = new DateTime(2026, 4, 6, 10, 0, 0),
                VehicleNumber = "DXB-T-24680",
                DriverName    = "Khalid Ibrahim",
                Material      = "Electronics",
                Supplier      = "Gulf Steel Industries",
                GrossWeight   = 15.0,
                TareWeight    = 0.0,
                Type          = WeighmentType.Inbound,
                Status        = WeighmentStatus.Pending,
                OperatorId    = "operator"
            },
            new WeighmentModel
            {
                TicketNumber  = "WB-2026-004",
                DateTime      = new DateTime(2026, 4, 5, 2, 20, 0),
                VehicleNumber = "DXB-T-13579",
                DriverName    = "Rashid Omar",
                Material      = "Automotive Parts",
                Supplier      = "Parts Trading LLC",
                GrossWeight   = 26.0,
                TareWeight    = 7.5,
                Type          = WeighmentType.Inbound,
                Status        = WeighmentStatus.Completed,
                OperatorId    = "operator"
            },
            new WeighmentModel
            {
                TicketNumber  = "WB-2026-005",
                DateTime      = new DateTime(2026, 4, 4, 11, 45, 0),
                VehicleNumber = "DXB-T-98765",
                DriverName    = "Abdullah Fahad",
                Material      = "Textiles",
                Supplier      = "Fabric World DMCC",
                GrossWeight   = 19.0,
                TareWeight    = 6.8,
                Type          = WeighmentType.Outbound,
                Status        = WeighmentStatus.Completed,
                OperatorId    = "operator"
            },
        };

        public Task<List<WeighmentModel>> GetAllWeighmentsAsync()
            => Task.FromResult(new List<WeighmentModel>(_store));

        public Task<WeighmentModel?> GetWeighmentByTicketAsync(string ticketNumber)
            => Task.FromResult(_store.FirstOrDefault(w => w.TicketNumber == ticketNumber));

        public Task<bool> SaveWeighmentAsync(WeighmentModel weighment)
        {
            _store.Add(weighment);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateWeighmentAsync(WeighmentModel weighment)
        {
            var idx = _store.FindIndex(w => w.TicketNumber == weighment.TicketNumber);
            if (idx < 0) return Task.FromResult(false);
            _store[idx] = weighment;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteWeighmentAsync(string ticketNumber)
        {
            var removed = _store.RemoveAll(w => w.TicketNumber == ticketNumber);
            return Task.FromResult(removed > 0);
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  MockVGMService
    // ═══════════════════════════════════════════════════════════
    public class MockVGMService : IVGMService
    {
        private static readonly List<VGMModel> _store = new()
        {
            new VGMModel
            {
                ContainerNumber = "DPWU7654321",
                VGMWeight       = 20.00,
                Method          = VGMMethod.Method1,
                SealNumber      = "SL789456",
                ShippingLine    = "Maersk Line",
                BookingNumber   = "MAE2026001",
                VerifiedBy      = "John Smith",
                Date            = new DateTime(2026, 4, 6, 9, 0, 0),
                Status          = VGMStatus.Certified
            },
            new VGMModel
            {
                ContainerNumber = "DPWU1234567",
                VGMWeight       = 18.50,
                Method          = VGMMethod.Method2,
                SealNumber      = "SL654321",
                ShippingLine    = "MSC",
                BookingNumber   = "MSC2026045",
                VerifiedBy      = "Sarah Johnson",
                Date            = new DateTime(2026, 4, 5, 15, 0, 0),
                Status          = VGMStatus.Certified
            }
        };

        public Task<List<VGMModel>> GetAllCertificatesAsync()
            => Task.FromResult(new List<VGMModel>(_store));

        public Task<bool> IssueCertificateAsync(VGMModel certificate)
        {
            _store.Add(certificate);
            return Task.FromResult(true);
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  MockHardwareService
    //  Simulates a weighbridge scale sending values every 200ms.
    //  Replace with SerialPortHardwareService for real hardware.
    // ═══════════════════════════════════════════════════════════
    public class MockHardwareService : IWeighbridgeHardwareService
    {
        public event Action<double>? WeightChanged;

        private System.Windows.Threading.DispatcherTimer? _timer;
        private readonly Random _rng = new();

        public void StartReading()
        {
            _timer = new System.Windows.Threading.DispatcherTimer
            { Interval = TimeSpan.FromMilliseconds(200) };
            _timer.Tick += (_, _) =>
            {
                double weight = Math.Round(15.0 + _rng.NextDouble() * 25.0, 2);
                WeightChanged?.Invoke(weight);
            };
            _timer.Start();
        }

        public void StopReading() => _timer?.Stop();
    }
}

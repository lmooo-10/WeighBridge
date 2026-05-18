using WeighBridge.Models;
using WeighBridge.Repositories.Interfaces;

namespace WeighBridge.Repositories
{
    public class MockWeighmentRepository : IWeighmentRepository
    {
        public Task<int> CountTodayAsync(string? type = null) =>
            Task.FromResult(type == "Import" ? 12 : 24);

        public Task<int> CountYesterdayAsync(string? type = null) =>
            Task.FromResult(type == "Import" ? 9 : 20);

        public Task<double> GetUptimePctAsync() =>
            Task.FromResult(99.4);

        public Task<List<ContainerTypeStat>> GetByTypeAndDirectionAsync() =>
            Task.FromResult(new List<ContainerTypeStat>
            {
                new() { ContainerType = "IMPU", WeighmentType = "Import", Count = 120 },
                new() { ContainerType = "EXPU", WeighmentType = "Export", Count = 85  },
                new() { ContainerType = "REEF", WeighmentType = "Import", Count = 40  },
                new() { ContainerType = "REEF", WeighmentType = "Export", Count = 30  },
                new() { ContainerType = "DGR",  WeighmentType = "Import", Count = 25  },
                new() { ContainerType = "DGR",  WeighmentType = "Export", Count = 18  },
            });

        public Task<List<WeighmentModel>> GetRecentAsync(int count = 10) =>
            Task.FromResult(new List<WeighmentModel>
            {
                new() { TicketNumber="WB-2026-001", DateTime=DateTime.Now.AddHours(-1),  VehicleNumber="16-ALG-1234", DriverName="Ali Benali",     Material="Steel Coils",            GrossWeight=28.5, TareWeight=8.5,  Type=WeighmentType.Import, Status=WeighmentStatus.Completed },
                new() { TicketNumber="WB-2026-002", DateTime=DateTime.Now.AddHours(-2),  VehicleNumber="09-ORA-5678", DriverName="Mohammed Kaci",   Material="Construction Materials",  GrossWeight=32.0, TareWeight=9.0,  Type=WeighmentType.Export, Status=WeighmentStatus.Completed },
                new() { TicketNumber="WB-2026-003", DateTime=DateTime.Now.AddHours(-3),  VehicleNumber="31-SET-9012", DriverName="Youcef Bouzid",   Material="Automotive Parts",        GrossWeight=26.0, TareWeight=7.5,  Type=WeighmentType.Import, Status=WeighmentStatus.Pending   },
                new() { TicketNumber="WB-2026-004", DateTime=DateTime.Now.AddHours(-5),  VehicleNumber="23-CON-3456", DriverName="Rashid Omar",     Material="Textiles",                GrossWeight=19.0, TareWeight=6.8,  Type=WeighmentType.Export, Status=WeighmentStatus.Completed },
                new() { TicketNumber="WB-2026-005", DateTime=DateTime.Now.AddHours(-8),  VehicleNumber="07-ALG-7890", DriverName="Ahmed Hassan",    Material="Electronics",             GrossWeight=15.0, TareWeight=4.2,  Type=WeighmentType.Import, Status=WeighmentStatus.Completed },
            });
    }
}
using WeighBridge.Models;
using WeighBridge.Repositories.Interfaces;

namespace WeighBridge.Repositories
{
    public class MockTOSRepository : ITOSRepository
    {
        public Task<int> GetImportVesselsCountAsync() => Task.FromResult(3);

        public Task<Vessel?> GetNextVesselAsync() =>
            Task.FromResult<Vessel?>(new Vessel
            {
                Id = 1,
                Name = "MSC AURORA",
                Origin = "Marseille",
                TEU = 850,
                ETA = DateTime.Now.AddHours(4),
                Status = "Import"
            });

        public Task<List<Vessel>> GetAllImportAsync() =>
            Task.FromResult(new List<Vessel>
            {
                new() { Id=1, Name="MSC AURORA",    Origin="Marseille",  TEU=850,  ETA=DateTime.Now.AddHours(4),  Status="Import" },
                new() { Id=2, Name="MAERSK ALGIERS", Origin="Barcelona",  TEU=1200, ETA=DateTime.Now.AddHours(12), Status="Import" },
                new() { Id=3, Name="CMA CGM ORAN",  Origin="Valencienne", TEU=640,  ETA=DateTime.Now.AddHours(20), Status="Import" },
            });
    }
}
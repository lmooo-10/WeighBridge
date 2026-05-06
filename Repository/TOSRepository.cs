using System.Collections.Generic;
using System.Threading.Tasks;
using WeighBridge.Models;
using WeighBridge.Repositories.Interfaces;

namespace WeighBridge.Repositories
{
    public class TOSRepository : ITOSRepository
    {
        // TODO: inject your DbContext / HttpClient here
        // private readonly AppDbContext _db;
        // public TOSRepository(AppDbContext db) => _db = db;

        public Task<int> GetImportVesselsCountAsync()
        {
            // TODO: query real data source
            throw new NotImplementedException();
        }

        public Task<Vessel?> GetNextVesselAsync()
        {
            // TODO: query real data source
            throw new NotImplementedException();
        }

        public Task<List<Vessel>> GetAllImportAsync()
        {
            // TODO: query real data source
            throw new NotImplementedException();
        }
    }
}
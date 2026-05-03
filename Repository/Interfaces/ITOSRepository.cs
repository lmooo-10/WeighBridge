using System.Collections.Generic;
using System.Threading.Tasks;
using WeighBridge.Models;

namespace WeighBridge.Repositories.Interfaces
{
    public interface ITOSRepository
    {
        Task<int> GetImportVesselsCountAsync();
        Task<Vessel?> GetNextVesselAsync();
        Task<List<Vessel>> GetAllImportAsync();
    }
}
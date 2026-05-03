using System.Collections.Generic;
using System.Threading.Tasks;
using WeighBridge.Models;

namespace WeighBridge.Repositories.Interfaces
{
    public interface IWeighmentRepository
    {
        Task<int> CountTodayAsync(string? type = null);
        Task<int> CountYesterdayAsync(string? type = null);
        Task<double> GetUptimePctAsync();
        Task<List<ContainerTypeStat>> GetByTypeAndDirectionAsync();
        Task<List<WeighmentModel>> GetRecentAsync(int count = 10);
    }
}

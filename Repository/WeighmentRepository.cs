using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeighBridge.Models;
using WeighBridge.Repositories.Interfaces;

namespace WeighBridge.Repositories
{
    public class WeighmentRepository : IWeighmentRepository
    {
        // TODO: inject your DbContext / HttpClient here
         //private readonly AppDbContext _db;
        // public WeighmentRepository(AppDbContext db) => _db = db;

        public Task<int> CountTodayAsync(string? type = null)
        {
            // TODO: query real data source
            throw new NotImplementedException();
        }

        public Task<int> CountYesterdayAsync(string? type = null)
        {
            // TODO: query real data source
            throw new NotImplementedException();
        }

        public Task<double> GetUptimePctAsync()
        {
            // TODO: query real data source
            throw new NotImplementedException();
        }

        public Task<List<ContainerTypeStat>> GetByTypeAndDirectionAsync()
        {
            // TODO: query real data source
            throw new NotImplementedException();
        }

        public Task<List<WeighmentModel>> GetRecentAsync(int count = 10)
        {
            // TODO: query real data source
            throw new NotImplementedException();
       }
    }
}
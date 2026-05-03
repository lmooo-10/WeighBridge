using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeighBridge.Models;
using WeighBridge.Repositories.Interfaces;

namespace WeighBridge.Repositories.Mock
{
    public class MockWeighmentRepository : IWeighmentRepository
    {
        private readonly List<WeighmentModel> _data;

        public MockWeighmentRepository()
        {
            var rng = new Random();
            var materials = new[] { "Céréales", "Engrais", "Machines", "Produits chimiques" };
            var suppliers = new[] { "CEVITAL", "SONATRACH", "CNAN", "NAFTAL" };

            _data = Enumerable.Range(0, 200).Select(i =>
            {
                // Use enum directly — no string array
                var type = i % 2 == 0 ? WeighmentType.Import : WeighmentType.Export;
                var gross = Math.Round(15.0 + rng.NextDouble() * 25.0, 2);
                var tare = Math.Round(gross * 0.3, 2);

                return new WeighmentModel
                {
                    TicketNumber = $"TK-2026-{10000 + i}",
                    DateTime = DateTime.Now
                                              .AddDays(-rng.Next(0, 2))
                                              .AddHours(-rng.Next(0, 8))
                                              .AddMinutes(-rng.Next(0, 60)),
                    VehicleNumber = $"16-{100 + i:D3}-{(char)('A' + rng.Next(26))}",
                    DriverName = $"Chauffeur {i + 1}",
                    ContainerNumber = $"{(type == WeighmentType.Import ? "IMPU" : "EXPU")}{100000 + i}",
                    Material = materials[rng.Next(materials.Length)],
                    Supplier = suppliers[rng.Next(suppliers.Length)],
                    GrossWeight = gross,
                    TareWeight = tare,
                    Type = type,
                    Status = WeighmentStatus.Completed,
                    OperatorId = "Agent Pesée"
                };
            }).ToList();
        }

        public Task<int> CountTodayAsync(string? type = null)
        {
            var q = _data.Where(w => w.DateTime.Date == DateTime.Today);
            if (type == "Import") q = q.Where(w => w.Type == WeighmentType.Import);
            if (type == "Export") q = q.Where(w => w.Type == WeighmentType.Export);
            return Task.FromResult(q.Count());
        }

        public Task<int> CountYesterdayAsync(string? type = null)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var q = _data.Where(w => w.DateTime.Date == yesterday);
            if (type == "Import") q = q.Where(w => w.Type == WeighmentType.Import);
            if (type == "Export") q = q.Where(w => w.Type == WeighmentType.Export);
            return Task.FromResult(q.Count());
        }

        public Task<double> GetUptimePctAsync()
            => Task.FromResult(99.8);

        public Task<List<ContainerTypeStat>> GetByTypeAndDirectionAsync()
        {
            var result = _data
                .GroupBy(w => new
                {
                    ContainerType = w.ContainerNumber != null && w.ContainerNumber.Length >= 4
                                    ? w.ContainerNumber[..4]
                                    : "UNKN",
                    WeighmentType = w.Type.ToString()
                })
                .Select(g => new ContainerTypeStat
                {
                    ContainerType = g.Key.ContainerType,
                    WeighmentType = g.Key.WeighmentType,
                    Count = g.Count()
                }).ToList();

            return Task.FromResult(result);
        }

        public Task<List<WeighmentModel>> GetRecentAsync(int count = 10)
        {
            var result = _data
                .OrderByDescending(w => w.DateTime)
                .Take(count)
                .ToList();

            return Task.FromResult(result);
        }
    }
}

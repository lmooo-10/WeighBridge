// Repositories/Mock/MockTOSRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeighBridge.Models;
using WeighBridge.Repositories.Interfaces;

public class MockTOSRepository : ITOSRepository
{
    private readonly List<Vessel> _vessels = new()
    {
        new Vessel
        {
            Id     = 1,
            Name   = "MSC AURORA",
            Origin = "Rotterdam",
            TEU    = 892,
            ETA    = DateTime.Today.AddHours(14).AddMinutes(30),
            Status = "Import"
        },
        new Vessel
        {
            Id     = 2,
            Name   = "CMA CGM ATLAS",
            Origin = "Santos, BR",
            TEU    = 1240,
            ETA    = DateTime.Today.AddHours(18).AddMinutes(0),
            Status = "Import"
        },
        new Vessel
        {
            Id     = 3,
            Name   = "EVER GIVEN",
            Origin = "Shanghai",
            TEU    = 650,
            ETA    = DateTime.Today.AddDays(1).AddHours(9),
            Status = "Import"
        }
    };

    public Task<int> GetImportVesselsCountAsync()
        => Task.FromResult(_vessels.Count(v => v.Status == "Import"));

    public Task<Vessel?> GetNextVesselAsync()
    {
        var next = _vessels
            .Where(v => v.ETA > DateTime.Now)
            .OrderBy(v => v.ETA)
            .FirstOrDefault();

        return Task.FromResult(next);
    }

    public Task<List<Vessel>> GetAllImportAsync()
        => Task.FromResult(_vessels.Where(v => v.Status == "Import").ToList());
}

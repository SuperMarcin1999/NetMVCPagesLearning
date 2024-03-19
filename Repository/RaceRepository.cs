using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMVCLearning.Data;
using NetMVCLearning.Models;
using NetMVCLearning.Repository.Interfaces;

namespace NetMVCLearning.Repository;

public class RaceRepository(ApplicationDbContext dbContext) : IRaceRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Race?>> GetAllAsync()
    {
        return await _dbContext.Races.ToListAsync();
    }

    public async Task<Race?> GetByIdAsync(int id)
    {
        return await _dbContext.Races
            .Include(r => r.Address)
            .FirstOrDefaultAsync(c => c!.Id == id);
    }
    
    [HttpPost]
    public async Task<Race?> GetByIdNoTrackingAsync(int id)
    {
        return await _dbContext.Races
            .Include(r => r.Address)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c!.Id == id);
    }

    public async Task<IEnumerable<Race>> GetByCityAsync(string city)
    {
        return await _dbContext.Races
            .Include(x => x.Address)
            .Where(c => c.Address.City == city)
            .ToListAsync();
    }

    public bool Add(Race race)
    {
        _dbContext.Races.Add(race);
        return Save();
    }

    public bool Update(Race race)
    {
        _dbContext.Races.Update(race);
        return Save();
    }

    public bool Delete(Race race)
    {
        _dbContext.Races.Remove(race);
        return Save();
    }

    public bool Save()
    {
        var result = _dbContext.SaveChanges();
        return result > 0;
    }
}
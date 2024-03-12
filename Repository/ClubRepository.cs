using Microsoft.EntityFrameworkCore;
using NetMVCLearning.Data;
using NetMVCLearning.Models;
using NetMVCLearning.Repository.Interfaces;

namespace NetMVCLearning.Repository;

public class ClubRepository(ApplicationDbContext dbContext) : IClubRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Club?>> GetAllAsync()
    {
        return await _dbContext.Clubs.ToListAsync();
    }

    public async Task<Club?> GetByIdAsync(int id)
    {
        return await _dbContext.Clubs
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c!.Id == id);
    }

    public async Task<IEnumerable<Club>> GetByCityAsync(string city)
    {
        return await _dbContext.Clubs
            .Include(x => x.Address)
            .Where(c => c.Address.City == city)
            .ToListAsync();
    }

    public bool Add(Club club)
    {
        _dbContext.Clubs.Add(club);
        return Save();
    }

    public bool Update(Club club)
    {
        _dbContext.Clubs.Update(club);
        return Save();
    }

    public bool Delete(Club club)
    {
        _dbContext.Clubs.Remove(club);
        return Save();
    }

    public bool Save()
    {
        var result = _dbContext.SaveChanges();
        return result > 0;
    }
}
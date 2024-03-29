﻿using NetMVCLearning.Models;

namespace NetMVCLearning.Repository.Interfaces;

public interface IRaceRepository
{
    Task<IEnumerable<Race?>> GetAllAsync();
    Task<Race?> GetByIdAsync(int id);
    Task<Race?> GetByIdNoTrackingAsync(int id);
    Task<IEnumerable<Race>> GetByCityAsync(string city);

    bool Add(Race club);
    bool Update(Race club);
    bool Delete(Race club);
    bool Save();
}
﻿using NetMVCLearning.Models;

namespace NetMVCLearning.Repository.Interfaces;

public interface IClubRepository
{
    Task<IEnumerable<Club?>> GetAllAsync();
    Task<Club?> GetByIdAsync(int id);
    Task<Club?> GetByIdNoTrackingAsync(int id);
    Task<IEnumerable<Club>> GetByCityAsync(string city);

    bool Add(Club club);
    bool Update(Club club);
    bool Delete(Club club);
    bool Save();
}
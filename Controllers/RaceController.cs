using Microsoft.AspNetCore.Mvc;
using NetMVCLearning.Models;
using NetMVCLearning.Repository.Interfaces;

namespace NetMVCLearning.Controllers;

public class RaceController(IRaceRepository raceRepository) : Controller
{
    private readonly IRaceRepository _raceRepository = raceRepository;

    // GET
    public async Task<IActionResult> Index()
    {
        var races = await _raceRepository.GetAllAsync();
        return View(races);
    }
    
    public async Task<IActionResult> Detail(int id)
    {
        var race = await _raceRepository.GetByIdAsync(id);
        return View(race);
    }
    
    public IActionResult Create(Race race)
    {
        if (!ModelState.IsValid)
        {
            return View(race);
        }

        _raceRepository.Add(race);
        return RedirectToAction("Index");
    }
}
using Microsoft.AspNetCore.Mvc;
using NetMVCLearning.Models;
using NetMVCLearning.Repository.Interfaces;
using NetMVCLearning.Services;
using NetMVCLearning.ViewModels;

namespace NetMVCLearning.Controllers;

public class RaceController(IRaceRepository raceRepository, IPhotoService photoService) : Controller
{
    private readonly IPhotoService _photoService = photoService;
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
    
    public async Task<IActionResult> Create(CreateRaceViewModel createRaceVM)
    {
        if (ModelState.IsValid)
        {
            var photoResult = await _photoService.AddPhotoAsync(createRaceVM.Image);
            
            var club = new Race()
            {
                Address = new Address()
                {
                    City = createRaceVM.City,
                    State = createRaceVM.State,
                    Street = createRaceVM.Street
                },
                Description = createRaceVM.Description,
                Title = createRaceVM.Title,
                RaceCategory = createRaceVM.RaceCategory,
                Image = photoResult.Url.ToString()
            };

            _raceRepository.Add(club);
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Photo upload failed");
        }

        return View(createRaceVM);
    }
}
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
            
            var race = new Race()
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

            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Photo upload failed");
        }

        return View(createRaceVM);
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var race = await _raceRepository.GetByIdAsync(id);
        if (race is null) return View("Error");

        var raceVM = new EditRaceViewModel()
        {
            Title = race.Title,
            RaceCategory = race.RaceCategory,
            Street = race.Address.Street,
            City = race.Address.City,
            Description = race.Description,
            State = race.Address.State,
            AddressId = race.Address.Id,
        };

        return View(raceVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditRaceViewModel editRaceVM)
    {
        var oldRace = await _raceRepository.GetByIdNoTrackingAsync(id);
        if (oldRace is null) return View("Error");

        try
        {
            var imageLocation = oldRace.Image;
            if (!string.IsNullOrEmpty(imageLocation))
            {
                await _photoService.DeletePhotoAsync(imageLocation);
            }
        }
        catch
        {
            ModelState.AddModelError("", "Error occured while deleting old image");
            return View(editRaceVM);
        }

        var photoUpload = await _photoService.AddPhotoAsync(editRaceVM.NewImage);

        var address = oldRace.Address;
        address.City = editRaceVM.City;
        address.Street = editRaceVM.Street;
        address.State = editRaceVM.State;
        
        var race = new Race()
        {
            Id = id,
            Description = editRaceVM.Description,
            Title = oldRace.Description,
            Image = photoUpload.Url.ToString(),
            AddressId = editRaceVM.AddressId,
            Address = address,
            RaceCategory = oldRace.RaceCategory,
        };

        _raceRepository.Update(race);
        return RedirectToAction("Index");
    }
}
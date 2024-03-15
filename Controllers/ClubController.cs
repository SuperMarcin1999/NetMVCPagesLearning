
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMVCLearning.Data;
using NetMVCLearning.Models;
using NetMVCLearning.Repository.Interfaces;
using NetMVCLearning.Services;
using NetMVCLearning.ViewModels;

namespace NetMVCLearning.Controllers;

public class ClubController(IClubRepository clubRepository, IPhotoService photoService) : Controller
{
    private readonly IClubRepository _clubRepository = clubRepository;
    private readonly IPhotoService _photoService = photoService;
    
    public async Task<IActionResult> Index()
    {
        var clubs = await _clubRepository.GetAllAsync();
        return View(clubs);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var club = await _clubRepository.GetByIdAsync(id);
        
        return View(club);
    }
    
    public async Task<IActionResult> Create(CreateClubViewModel createClubVM)
    {
        if (ModelState.IsValid)
        {
            var photoResult = await _photoService.AddPhotoAsync(createClubVM.Image);
            
            var club = new Club()
            {
                Address = new Address()
                {
                    City = createClubVM.City,
                    State = createClubVM.State,
                    Street = createClubVM.Street
                },
                Description = createClubVM.Description,
                Title = createClubVM.Title,
                ClubCategory = createClubVM.ClubCategory,
                Image = photoResult.Url.ToString()
            };

            _clubRepository.Add(club);
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Photo upload failed");
        }

        return View(createClubVM);
    }
}

using CloudinaryDotNet.Actions;
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

    public async Task<IActionResult> Edit(int id)
    {
        var oldClub = await _clubRepository.GetByIdAsync(id);
        if (oldClub is null) return View("Error");

        var clubVM = new EditClubViewModel()
        {
            Title = oldClub.Title,
            ClubCategory = oldClub.ClubCategory,
            Street = oldClub.Address.Street,
            City = oldClub.Address.City,
            Description = oldClub.Description,
            State = oldClub.Address.State,
            AddressId = oldClub.Address.Id,
        };

        return View(clubVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditClubViewModel editClubVM)
    {
        var oldClub = await clubRepository.GetByIdNoTrackingAsync(id);
        if (oldClub is null) return View("Error");

        try
        {
            var imageLocation = oldClub.Image;
            if (!string.IsNullOrEmpty(imageLocation))
            {
                await _photoService.DeletePhotoAsync(imageLocation);
            }
        }
        catch
        {
            ModelState.AddModelError("", "Error occured while deleting old image");
            return View(editClubVM);
        }

        var photoUpload = await _photoService.AddPhotoAsync(editClubVM.NewImage);

        var address = oldClub.Address;
        address.City = editClubVM.City;
        address.Street = editClubVM.Street;
        address.State = editClubVM.State;
        
        var club = new Club()
        {
            Id = id,
            Description = editClubVM.Description,
            Title = oldClub.Description,
            Image = photoUpload.Url.ToString(),
            AddressId = editClubVM.AddressId,
            Address = address,
            ClubCategory = oldClub.ClubCategory,
        };

        _clubRepository.Update(club);
        return RedirectToAction("Index");
    }
 }
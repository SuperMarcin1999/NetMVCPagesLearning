
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMVCLearning.Data;
using NetMVCLearning.Models;
using NetMVCLearning.Repository.Interfaces;

namespace NetMVCLearning.Controllers;

public class ClubController(IClubRepository clubRepository) : Controller
{
    private readonly IClubRepository _clubRepository = clubRepository;

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
    
    public IActionResult Create(Club club)
    {
        if (!ModelState.IsValid)
        {
            return View(club);
        }

        _clubRepository.Add(club);
        return RedirectToAction("Index");
    }
}
﻿using NetMVCLearning.Data.Enum;

namespace NetMVCLearning.ViewModels;

public class EditClubViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile NewImage { get; set; }
    public ClubCategory ClubCategory { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    
    public int AddressId { get; set; }
}
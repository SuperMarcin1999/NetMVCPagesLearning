using NetMVCLearning.Data.Enum;

namespace NetMVCLearning.ViewModels;

public class CreateRaceViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
    public RaceCategory RaceCategory { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}
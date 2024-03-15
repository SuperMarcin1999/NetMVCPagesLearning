using NetMVCLearning.Data.Enum;

namespace NetMVCLearning.ViewModels;

public class CreateClubViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
    public ClubCategory ClubCategory { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}
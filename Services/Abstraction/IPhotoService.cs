using CloudinaryDotNet.Actions;

namespace NetMVCLearning.Services;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}
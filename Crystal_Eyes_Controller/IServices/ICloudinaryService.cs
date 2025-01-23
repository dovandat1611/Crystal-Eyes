using CloudinaryDotNet.Actions;

namespace Crystal_Eyes_Controller.IServices
{
	public interface ICloudinaryService
	{
		Task<ImageUploadResult?> UploadImageAsync(IFormFile file);
		Task<List<ImageUploadResult>> UploadImagesAsync(List<IFormFile> files);
	}
}

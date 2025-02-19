using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace ChildCareCalendar.Utilities.Helper
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, fileStream),
                    PublicId = "uploads/" + Path.GetFileNameWithoutExtension(fileName),
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi upload Cloudinary: " + ex.Message);
                return null;
            }
        }
    }
}

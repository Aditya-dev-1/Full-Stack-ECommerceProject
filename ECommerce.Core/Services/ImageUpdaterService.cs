using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using ServiceContracts;

namespace Services
{
    public class ImageUpdaterService : IImageUpdaterService
    {
        private readonly IImageDeleterService _imageDeleterService;
        private readonly IHostEnvironment _hostEnvironment;

        public ImageUpdaterService(IImageDeleterService imageDeleterService, IHostEnvironment hostEnvironment)
        {
            _imageDeleterService = imageDeleterService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> ImageUpdater(IFormFile imageFile, string existingUrl, string subFolder = "Images")
        {
            
            if (string.IsNullOrWhiteSpace(existingUrl))
                throw new ArgumentException("Existing image URL cannot be null or empty");

            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("New image file is required");

            // Get wwwroot path
            var webRootPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot");

            // Ensure images directory exists
            var imagesDirectory = Path.Combine(webRootPath, subFolder);
            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }

            // Generate new filename and paths
            var newFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            var newRelativePath = Path.Combine(subFolder, newFileName);
            var newAbsolutePath = Path.Combine(webRootPath, newRelativePath);

            // Save new image
            using (var stream = new FileStream(newAbsolutePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Delete old image if it exists
            if (!string.IsNullOrWhiteSpace(existingUrl))
            {
                await _imageDeleterService.ImageDeleter(existingUrl);
            }

            // Return new relative URL (with forward slashes)
            return $"/{newRelativePath.Replace("\\", "/")}";

        }
    }
}

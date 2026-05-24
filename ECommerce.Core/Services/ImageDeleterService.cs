using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ServiceContracts;

namespace Services
{
    public class ImageDeleterService : IImageDeleterService
    {
        
        private readonly IHostEnvironment _hostEnvironment;
        public ImageDeleterService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<bool> ImageDeleter(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return false;


            // Removing leading slash if present
            var cleanPath = imagePath.TrimStart('/', '\\');

            // Combining with ContentRootPath and manually add wwwroot
            var fullPath = Path.Combine(
                _hostEnvironment.ContentRootPath,
                "wwwroot",
                cleanPath);

            // Normalizing path separators
            fullPath = fullPath.Replace('/', Path.DirectorySeparatorChar)
                              .Replace('\\', Path.DirectorySeparatorChar);

            // Checking if file exists
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
    }
}

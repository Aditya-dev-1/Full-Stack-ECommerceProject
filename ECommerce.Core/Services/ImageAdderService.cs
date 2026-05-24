using System;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using ServiceContracts;



namespace Services
{
    public class ImageAdderService : IImageAdderService
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ImageAdderService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> ImageAdder(IFormFile imageFile, string subFolder = "Images")
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageFile), "Image file cannot be null or empty");
            }

            // Getting wwwroot path by combining ContentRootPath with wwwroot
            var wwwrootPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot");

            // Create target directory if it doesn't exist
            var targetFolder = Path.Combine(wwwrootPath, subFolder);
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            // Generating unique filename
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            var filePath = Path.Combine(targetFolder, uniqueFileName);

            // Saving the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

          
            return $"/{subFolder}/{uniqueFileName}";
        }

       
    }
}


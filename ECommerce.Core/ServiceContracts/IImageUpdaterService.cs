using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    public interface IImageUpdaterService
    {
        /// <summary>
        /// Updates the image
        /// </summary>
        /// <param name="imageFile">the new image</param>
        /// <param name="existingUrl">the existing image url</param>
        /// <param name="subFolder">the subfolder in webfolder if any</param>
        /// <returns>Returns new image url</returns>
        Task<string> ImageUpdater(IFormFile imageFile, string existingUrl,string subFolder="Images");
    }
}

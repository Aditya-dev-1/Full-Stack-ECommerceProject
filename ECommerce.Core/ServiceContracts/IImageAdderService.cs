using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    public interface IImageAdderService
    {
        /// <summary>
        /// Adds image in the web folder
        /// </summary>
        /// <param name="imageFile">the image to add</param>
        /// <returns>Returns the path of image </returns>
        Task<string> ImageAdder(IFormFile imageFile, string subFolder="Images");

        
    }
}

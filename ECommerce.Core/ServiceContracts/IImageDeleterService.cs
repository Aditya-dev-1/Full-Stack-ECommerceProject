using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    public interface IImageDeleterService
    {
        
        /// <summary>
        /// Delete image from web folder
        /// </summary>
        /// <param name="imagePath">the image path to delete</param>
        /// <returns>Returns true if deleted; otherwise false</returns>
        Task<bool> ImageDeleter(string imagePath);
    }
}

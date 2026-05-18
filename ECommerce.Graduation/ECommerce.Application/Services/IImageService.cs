using ECommerce.Application.DTOs.Images;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IImageService
    {
        Task<ImageResultResponse> UploadAsync(IFormFile Image);

        Task DeleteAsync(string publicId);
    }
}

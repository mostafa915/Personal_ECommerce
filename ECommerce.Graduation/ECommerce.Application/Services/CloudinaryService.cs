using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ECommerce.Application.DTOs.Images;
using ECommerce.Application.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class CloudinaryService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var account = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageResultResponse> UploadAsync(IFormFile Image)
        {
            using var stream = Image.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(Image.FileName, stream),
                Folder = "products"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return new(result.SecureUrl.ToString(), result.PublicId);
        }


        public async Task DeleteAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}

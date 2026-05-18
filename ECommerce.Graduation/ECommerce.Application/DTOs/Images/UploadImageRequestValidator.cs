using ECommerce.Application.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Images
{
    public class UploadImageRequestValidator : AbstractValidator<IFormFile>
    {
        public UploadImageRequestValidator()
        {

            RuleFor(x => x)
                .Must((request, context) => ImageSettings.AllowExtensions.Contains(Path.GetExtension(request.FileName).ToLower()))
                .WithMessage("Not allowed to upload this file extension!")
                .When(x => x is not null);


            RuleFor(x => x)
                .Must((request, context) => request.Length <= ImageSettings.MaxImageSizeBytes)
                .WithMessage($"Max file size is {ImageSettings.MaxImageSizeMB} MB!")
                .When(x => x is not null);
        }
    }
}

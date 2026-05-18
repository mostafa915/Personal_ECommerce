using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Settings
{
    public static class ImageSettings
    {
        public const int MaxImageSizeMB = 5;
        public const int MaxImageSizeBytes  = MaxImageSizeMB * 1024 * 1024;
        public static readonly List<string> AllowExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    }
}

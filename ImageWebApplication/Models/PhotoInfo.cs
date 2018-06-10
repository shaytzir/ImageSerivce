using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageWebApplication.Models
{
    public class PhotoInfo
    {
        public PhotoInfo(string thumbFullPath, string outputDirName) 
        {
            try
            {
                OutputDir = outputDirName;
                ImageFullThumbnailUrl = thumbFullPath;
                ImageFullUrl = thumbFullPath.Replace(@"Thumbnails\", string.Empty);
                PhotoName = Path.GetFileNameWithoutExtension(ImageFullThumbnailUrl);
                Month = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(ImageFullThumbnailUrl));
                Year = Path.GetFileNameWithoutExtension(Path.GetDirectoryName((Path.GetDirectoryName(ImageFullThumbnailUrl))));
                ImageRelativePathThumbnail = @"~\" + Path.GetFileName(outputDirName) + thumbFullPath.Replace(outputDirName, string.Empty);
                ImageRelativePath = ImageRelativePathThumbnail.Replace(@"Thumbnails\", string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string PhotoName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageUrl")]
        public string ImageFullThumbnailUrl { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageRelativePathThumbnail")]
        public string ImageRelativePathThumbnail { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageRelativePath")]
        public string ImageRelativePath { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageFullUrl")]
        public string ImageFullUrl { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        [Display(Name ="OutputDir")]
        public string OutputDir { get; set; }
    }
}
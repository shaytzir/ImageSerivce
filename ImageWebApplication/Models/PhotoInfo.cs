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
                PhotoThumbFullUrl = thumbFullPath;
                PhotoFullUrl = thumbFullPath.Replace(@"Thumbnails\", string.Empty);
                PhotoName = Path.GetFileNameWithoutExtension(PhotoThumbFullUrl);
                Month = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(PhotoThumbFullUrl));
                Year = Path.GetFileNameWithoutExtension(Path.GetDirectoryName((Path.GetDirectoryName(PhotoThumbFullUrl))));
                PhotoThumbRelativePath = @"~\" + Path.GetFileName(outputDirName) + thumbFullPath.Replace(outputDirName, string.Empty);
                PhotoRelativePath = PhotoThumbRelativePath.Replace(@"Thumbnails\", string.Empty);
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
        [Display(Name = "OutputDir")]
        public string OutputDir { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "PhotoFullUrl")]
        public string PhotoFullUrl { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageUrl")]
        public string PhotoThumbFullUrl { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "PhotoRelativePath")]
        public string PhotoRelativePath { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "PhotoThumbRelativePath")]
        public string PhotoThumbRelativePath { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Web;

namespace ImageWebApplication.Models
{
    public class Photos
    {
        private string outputDir;
        private static string[] extends = { ".jpg", ".bmp", ".png", ".gif" };
        public Photos()
        {
            ImageList = new List<PhotoInfo>();
            //this.outputDir = outputDirectory;
            //this.GetAllPhotos();
        }

        public List<PhotoInfo> ImageList
        {
            get; set;
        }

        public void GetAllPhotos(string outputDirectory)
        {
            this.outputDir = outputDirectory;
            string outputDirThumb = Path.Combine(this.outputDir, "Thumbnails");
            DirectoryInfo thumbDirInfo = new DirectoryInfo(outputDirThumb);
            foreach (DirectoryInfo year in thumbDirInfo.GetDirectories())
            {
                foreach (DirectoryInfo month in year.GetDirectories())
                {
                    foreach (FileInfo thumb in month.GetFiles())
                    {
                        if (extends.Contains(thumb.Extension.ToLower()))
                        {
                            try
                            {
                                string thumbRelPath= @"~\" + Path.GetFileName(outputDir) + thumb.FullName.Replace(outputDir, string.Empty);
                                string name = thumb.Name;
                                string photoRelPath = thumbRelPath.Replace("Thumbnails\\", string.Empty);
                                int yearInt = int.Parse(year.Name);
                                int monthInt = int.Parse(month.Name);
                                PhotoInfo photo = new PhotoInfo(thumb.FullName, outputDir);
                                this.ImageList.Add(photo);
                                //                    PhotoInfo photo = new PhotoInfo(name, path, thumbnailPath, year, month, thumb.FullName);
                                //                  this.ImageList.Add(photo);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }

                    }
                }
            }
        }
    }
}
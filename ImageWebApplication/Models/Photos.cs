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
            PhotosList = new List<PhotoInfo>();
        }

        public List<PhotoInfo> PhotosList
        {
            get; set;
        }

        public int PhotosNum
        {
            get { return this.PhotosList.Count(); }
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
                                PhotoInfo photo = new PhotoInfo(thumb.FullName, outputDir);
                                this.PhotosList.Add(photo);
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


        public void DeletePhoto(string thumbUrl)
        {
            try
            {
                foreach (PhotoInfo photo in PhotosList)
                {
                    if (photo.PhotoThumbFullUrl.Equals(thumbUrl))
                    {
                        this.PhotosList.Remove(photo);
                        File.Delete(photo.PhotoFullUrl);
                        File.Delete(photo.PhotoThumbFullUrl);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
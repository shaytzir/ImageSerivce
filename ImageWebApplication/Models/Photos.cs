using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Web;

namespace ImageWebApplication.Models
{
    /// <summary>
    /// saving all photos in the service outputdir
    /// </summary>
    public class Photos
    {
        private string outputDir;
        private static string[] extensions = { ".jpg", ".bmp", ".png", ".gif" };
        /// <summary>
        /// constructor
        /// </summary>
        public Photos()
        {
            PhotosList = new List<PhotoInfo>();
        }

        public List<PhotoInfo> PhotosList
        {
            get; set;
        }

        /// <summary>
        /// sets the list to hold all photo infos from the outputdir
        /// </summary>
        /// <param name="outputDirectory">The output directory.</param>
        public void GetAllPhotos(string outputDirectory)
        {
            this.outputDir = outputDirectory;
            string outputDirThumb = Path.Combine(this.outputDir, "Thumbnails");
            DirectoryInfo thumbDirInfo = new DirectoryInfo(outputDirThumb);
            //going through all the sub directories inside the thumbnail directory
            foreach (DirectoryInfo year in thumbDirInfo.GetDirectories())
            {
                foreach (DirectoryInfo month in year.GetDirectories())
                {
                    foreach (FileInfo thumb in month.GetFiles())
                    {
                        if (extensions.Contains(thumb.Extension.ToLower()))
                        {
                            try
                            {
                                //create a new photo info and add it to the list
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

        /// <summary>
        /// Deletes the photo.
        /// </summary>
        /// <param name="thumbUrl">The thumb URL.</param>
        public void DeletePhoto(string thumbUrl)
        {
            try
            {
                foreach (PhotoInfo photo in PhotosList)
                {
                    if (photo.PhotoThumbFullUrl.Equals(thumbUrl))
                    {
                        //remove the photo from the list
                        this.PhotosList.Remove(photo);
                        //delete actual files from the directories
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
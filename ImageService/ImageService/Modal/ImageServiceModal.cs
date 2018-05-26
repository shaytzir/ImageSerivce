using ImageService.Logging;
using Infrastructure;
using Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace ImageService.Modal
{
    /// <summary>
    /// class of the actual service modal - implents the interface
    /// </summary>
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        // The Output Folder
        private string m_OutputFolder;
        // The Size Of The Thumbnail Size
        private int m_thumbnailSize;
        private static Regex r = new Regex(":");
        #endregion

        /// <summary>
        /// getter/setter
        /// </summary>
        public string OutputFolder
        {
            // The Output Folder
            get
            {
                return this.m_OutputFolder;
            }
            set
            {
                this.m_OutputFolder = value;
            }
        }

        /// <summary>
        /// getter setter for the thumbnail size
        /// </summary>
        public int ThumbnailSize
        {
            get
            {
                return this.m_thumbnailSize;
            }
            set
            {
                this.m_thumbnailSize = value;
            }
        }
        ILoggingService m_logger;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="m_OutputFolder">the path of the output folder</param>
        /// <param name="m_thumbnailSize">the size of the created thumbnail</param>
        public ImageServiceModal(string m_OutputFolder, int m_thumbnailSize, ILoggingService logger)
        {
            this.m_OutputFolder = m_OutputFolder;
            this.m_thumbnailSize = m_thumbnailSize;
            this.m_logger = logger;
        }

        /// <summary>
        /// add file function -creates year/month and thumbnail/year/month directories
        /// and move there the added file + a new thumbnail to it's directory
        /// </summary>
        /// <param name="path">the path of the original file</param>
        /// <param name="result">true/false - success of adding/error message</param>
        /// <returns></returns>
        public string AddFile(string path, out bool result)
        {
            string message;
            try
            {
                //if the destanation directory isnt open - create one
                CreateOutPutDirectory();
                //same for the thumbnail directory inside it
                CreateThumbnailsDirectory();
                //if the original file exists
                if (File.Exists(path))
                {
                    result = true;
                    //Extract file Year and Month
                    DateTime creationTime = GetDateTakenFromImage(path);
                    //create string of the year
                    string yearCreation = string.Empty;
                    yearCreation = creationTime.Year.ToString();
                    //create string of the month
                    string monthCreation = string.Empty;
                    monthCreation = creationTime.Month.ToString();
                    //create direcory = DestenationDirectory/Year/Month
                    CreateYearMonthDirectory(yearCreation, monthCreation);
                    //create directory = DestanationDirectory/Thumbnails/Year/Month
                    CreateThumbnailsYearMonth(yearCreation, monthCreation);

                    //get the path Destenation/Year/Month
                    string destImagePath = GetYearMonthPath(yearCreation, monthCreation);
                    //get the path Destenation/Thumbnail/Year/Month
                    string destThumbPath = GetThumbnailYearMonthPath(yearCreation, monthCreation);
                    //save a thumbnail file in relevant direcory
                    SaveThumbnailInOutputDir(path, destThumbPath);
                    //and move the original file to the relevant directory
                    MoveFileToDirectory(path, destImagePath);
                }
                //if the originial file doesnt exists
                else
                {
                    result = false;
                    message = path + " Does Not Exist! didnt saved into " + m_OutputFolder;
                    return message;
                }
                //in any other case of failure - catch it and log as faliure
            }
            catch (Exception e)
            {
                result = false;
                message = e.Message;
                return message;
            }
            //in any other case the transfer worked - log it
            result = true;
            message = Path.GetFileName(path) + " was succesfully moved from " + path +
                " into " + m_OutputFolder + " relevant directories";
            return message;
        }

        /// <summary>
        /// creating a thumbnail and saving it into the right directory
        /// </summary>
        /// <param name="path">original path of file</param>
        /// <param name="destThumbPath">destenation/Thumbnail/Year/Month</param>
        private void SaveThumbnailInOutputDir(string path, string destThumbPath)
        {
            string ext = Path.GetExtension(path);

            int i = 1;
            //combines the path to also have the file name
            string destPathFileName = Path.Combine(destThumbPath, Path.GetFileName(path));

            string newDestName = destPathFileName;
            //if we're trying to save a thumbnail with a name that already exists, add to 
            //it's name 1/2/3... according to the copy number
            while (File.Exists(newDestName))
            {
                newDestName = destThumbPath;
                string name = Path.GetFileNameWithoutExtension(path) + i.ToString() + ext;
                newDestName = Path.Combine(destThumbPath, name);
                i++;
            }

            //save the thumbnail in it's directory
            using (Image pic = Image.FromFile(path))
            using (Image thumbnail = pic.GetThumbnailImage
                (this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero))
            {
                thumbnail.Save(newDestName);
                thumbnail.Dispose();
            }

        }


        /// <summary>
        /// extract the date of the in which the file was created
        /// </summary>
        /// <param name="path">original file path</param>
        /// <returns>a date</returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            DateTime creation;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                //if there is info about the creation time return in
                if (myImage.PropertyIdList.Any(p => p == 36867))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    creation = DateTime.Parse(dateTaken);
                }
                //if no info - return by default todays date
                else
                {
                    creation = File.GetCreationTime(path);
                }

            }
            return creation;
        }

        /// <summary>
        /// the output direcoty - create if doesnt exists
        /// </summary>
        public void CreateOutPutDirectory()
        {
            try
            {
                if (!Directory.Exists(this.m_OutputFolder))
                {
                    Directory.CreateDirectory(m_OutputFolder).Attributes |= FileAttributes.Hidden;
                }
            }
            catch (Exception e)
            {
                throw new Exception("wasn't able to find or create " + m_OutputFolder + " ");
            }

        }

        /// <summary>
        /// create the direcory Thumbnails inside the output directory
        /// </summary>
        public void CreateThumbnailsDirectory()
        {
            string thumbPath = Path.Combine(m_OutputFolder, "Thumbnails");
            try
            {
                Directory.CreateDirectory(thumbPath);
            }
            catch (Exception e)
            {
                throw new Exception("wasn't able to create " + thumbPath + " ");
            }

        }

        /// <summary>
        /// creates the year and month direcories inside the outputDir
        /// </summary>
        /// <param name="year">year in string</param>
        /// <param name="month">month in string</param>
        public void CreateYearMonthDirectory(string year, string month)
        {
            //get the path outputDir/Year/Month
            string regularYearMonth = GetYearMonthPath(year, month);
            try
            {
                //try to create the direcories
                Directory.CreateDirectory(regularYearMonth);
            }
            catch (Exception e)
            {
                throw new Exception("failed. wasn't able to create " + regularYearMonth + " directory ");
            }
        }

        /// <summary>
        /// create the outputDir/Thumbnails/Year/Month
        /// </summary>
        /// <param name="year">year in string</param>
        /// <param name="month">month in string</param>
        public void CreateThumbnailsYearMonth(string year, string month)
        {
            //get the path outPut/Thumbnail/Year/Month
            string thumbYearMonth = GetThumbnailYearMonthPath(year, month);
            try
            {
                //try to create it
                Directory.CreateDirectory(thumbYearMonth);
            }
            catch
            {
                throw new Exception("failed. wasn't able to create " + thumbYearMonth + " directory ");
            }
        }

        /// <summary>
        /// return the path outputDir/Year/Month
        /// </summary>
        /// <param name="year">year in string</param>
        /// <param name="month">month in string</param>
        /// <returns>"outputDir/Year/Month"</returns>
        public string GetYearMonthPath(string year, string month)
        {
            return Path.Combine(m_OutputFolder, year, month);
        }

        /// <summary>
        /// returns th path outPutDir/Thumbnails/Year/Month
        /// </summary>
        /// <param name="year">year in string</param>
        /// <param name="month">month in string</param>
        /// <returns>"outPutDir/Thumbnails/Year/Month"</returns
        public string GetThumbnailYearMonthPath(string year, string month)
        {
            return Path.Combine(m_OutputFolder, "Thumbnails", year, month);
        }

        /// <summary>
        /// moving the original file into it's destenation relevant direcotry
        /// </summary>
        /// <param name="sourcePath">original file path</param>
        /// <param name="destPath">"outputDir/Year/Month"</param>
        public void MoveFileToDirectory(string sourcePath, string destPath)
        {
            int i = 1;
            string ext = Path.GetExtension(sourcePath);
            string fileName = Path.GetFileName(sourcePath);
            string destFilePath = Path.Combine(destPath, fileName);

            string newDestName = destFilePath;
            //as long as the direcory contains a file w the same name
            //add 1/2/3... according to the copy number
            while (File.Exists(newDestName))
            {
                newDestName = destPath;
                string name = Path.GetFileNameWithoutExtension(sourcePath) + i.ToString() + ext;
                newDestName = Path.Combine(destPath, name);
                i++;
            }
            try
            {
                File.Move(sourcePath, newDestName);
            }
            catch
            {
                throw new Exception("couldnt move " + sourcePath + " to " + newDestName);
            }
        }


        /// <summary>
        /// check if the outputdir already exsists
        /// </summary>
        /// <param name="sourcePath">original file path</param>
        /// <param name="destPath">dest path</param>
        /// <returns>true if exsitis - otherwise false</returns>
        public bool CheckIfDestExists(string sourcePath, string destPath)
        {

            string fileName = Path.GetFileName(sourcePath);
            string destFilePath = Path.Combine(destPath, fileName);
            //if the file wasnt moved to it's destenation folders before
            if (!File.Exists(destFilePath))
            {
                return false;
            }
            return true;
        }

        public string GetConfig(out bool result)
        {
            result = true;
            
        //    CommandInfo info = new CommandInfo();
         //   info.CommandID = (int)CommandEnum.GetConfigCommand;
            JObject obj = new JObject();
            obj["CommandID"] = (int)CommandEnum.GetConfigCommand;
            //split all directories to handle from the appconfig
            obj["Handlers"] = ConfigurationManager.AppSettings["Handler"];
            //save the outputDir from appconfig
            obj["OutputDir"] = ConfigurationManager.AppSettings["outputDir"];
            //save the source name from appconfig
            obj["SourceName"] = ConfigurationManager.AppSettings["SourceName"];
            //save the log name from appconfig
            obj["LogName"] = ConfigurationManager.AppSettings["LogName"];
            //extract thumbnail size from appconfig
            obj["ThumbSize"] = ConfigurationManager.AppSettings["ThumbnailSize"];
           // info.CommandArgs = obj;
            string output = JsonConvert.SerializeObject(obj);
            return output;
        }
        public string GetLog(out bool result)
        {
            result = true;

            //    CommandInfo info = new CommandInfo();
            //   info.CommandID = (int)CommandEnum.GetConfigCommand;
            JObject obj = new JObject();
            obj["CommandID"] = (int)CommandEnum.LogCommand;
            obj["LogList"] = JsonConvert.SerializeObject(this.m_logger.LogList);
            //obj["LogList"] = (JArray)JToken.FromObject(this.m_logger.LogList);
            string output = JsonConvert.SerializeObject(obj);
            return output;
        }
            public string RemoveHandlerFromConfig(string handler, out bool result)
        {
            result = true;
            Configuration m_Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string oldHandlersConnected = ConfigurationManager.AppSettings["Handler"];
            string[] oldHandlers = oldHandlersConnected.Split(';');
           m_Configuration.AppSettings.Settings.Remove("Handler");
            StringBuilder newHandlers = new StringBuilder();
            foreach (string h in oldHandlers)
            {
                if (!h.Equals(handler))
                {
                    newHandlers.Append(h);
                    newHandlers.Append(';');
                }
            }
            if (newHandlers.Length > 1)
            {
                newHandlers.Remove(newHandlers.Length - 1, 1);
            }
            string newHandlesString = newHandlers.ToString();
            ConfigurationManager.AppSettings.Set("Handler", newHandlesString);
         //   return "removed " + handler + " from appConfig";


            /*Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["Handler"].Value = newHandlesString;
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");*/
            return "removed " + handler + " from appConfig";
        }
    }
}

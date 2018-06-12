using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ImageWebApplication.Communication;
using System.Windows;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ImageWebApplication.Models
{
    public class AppConfig 
    {
        private WebClient client;
        private object objLock;
        private int numOfPhotos;
        public bool InitDone { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AppConfig()
        {
            //object to use as a lock
            this.objLock = new object();
            //the first initialize of the condig wasnt done yet
            this.InitDone = false;
            this.client = WebClient.Instance;
            this.Handlers = new ObservableCollection<string>();
            //when the client recieves informtaion from the server call the handle function
            client.Comm.InfoFromServer += HandleServerCommands;
        }

        /// <summary>
        /// Asks to remove handler. ask the server to remove and blocks untill
        /// </summary>
        /// <param name="jsonCommandInfo">The json command information.</param>
        public void AskToRemoveHandler(string jsonCommandInfo)
        {
            this.client.Comm.SendCommand(jsonCommandInfo);
            //waits for the function coming from event to end (making it a sync communication)
            WaitWithBlock();
        }

        /// <summary>
        /// Waits on the special object
        /// </summary>
        public void WaitWithBlock()
        {
            lock (this.objLock)
            {
                Monitor.Wait(this.objLock);
            }
            return;
        }

        /// <summary>
        /// Pulses the blocking.
        /// </summary>
        public void PulseTheBlocking()
        {
            lock (this.objLock)
            {
                Monitor.Pulse(this.objLock);
            }
        }
        /// <summary>
        /// Handles the server commands.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="commandFromSrv">The command the server sent</param>
        private void HandleServerCommands(object sender, string commandFromSrv)
           {
            JObject json = JsonConvert.DeserializeObject<JObject>(commandFromSrv);
            int commandID = (int)json["CommandID"];
            //if it send the configuration info
            if (commandID == (int)CommandEnum.GetConfigCommand)
            {
                UpdateConfig(commandFromSrv);
                //if it closed a directory
            }
            else if (commandID == (int)CommandEnum.CloseCommand)
            {
                RemoveHandler(commandFromSrv);
            }
           }


        /// <summary>
        /// Removes the handler.
        /// </summary>
        /// <param name="commanFromSrv">The MSG.</param>
        public void RemoveHandler(string commanFromSrv)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(commanFromSrv);
            try
            {
                string handlerToRemove = (string)json["Handler"];
                Handlers.Remove(handlerToRemove);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                lock (this.objLock)
                {
                    Monitor.Pulse(this.objLock);
                }
            }
        }


        /// <summary>
        /// Updates the configuration.
        /// </summary>
        /// <param name="CommandFromSrv">The MSG.</param>
        private void UpdateConfig(string CommandFromSrv)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(CommandFromSrv);
            try
            {
                this.LogName = (string)json["LogName"];
                string handlersConnected = (string)json["Handlers"];
                string[] handlers = handlersConnected.Split(';');
                if (!handlers[0].Equals(""))
                {
                    for (int i = 0; i < handlers.Length; i++)
                    {
                        this.Handlers.Add(handlers[i]);
                    }
                }
                this.SourceName = (string)json["SourceName"];
                this.OutputDir = (string)json["OutputDir"];
                this.ThumbnailSize = (string)json["ThumbSize"];
                this.NumOfPhotos = getPhotosNum();
                //knowing the first initialize of this model was done
                this.InitDone = true;
                //alerting the config building - done
                this.PulseTheBlocking();
            }
            catch (Exception e)
            {
                return;
            }
        }

        /// <summary>
        /// Gets the photos number.
        /// </summary>
        /// <returns></returns>
        private int getPhotosNum()
        {
            int NumOfPhotos = 0;
            string path = Path.Combine(this.OutputDir, "Thumbnails");
            //counting the files in the thumbnails path
            if (Directory.Exists(path))
            {
                NumOfPhotos = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).Count();
            }
            return NumOfPhotos;

        }

        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>
        /// The output dir.
        /// </value>
        public string OutputDir { get; set; }


        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>
        /// The name of the log.
        /// </value>
        public string LogName { get; set; }


        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        public string SourceName { get; set; }

        /// <summary>
        /// Gets or sets the size of the thumbnail.
        /// </summary>
        /// <value>
        /// The size of the thumbnail.
        /// </value>
        public string ThumbnailSize { get; set; }

        /// <summary>
        /// Gets or sets the handlers.
        /// </summary>
        /// <value>
        /// The handlers.
        /// </value>
        public ObservableCollection<string> Handlers { get; set; }

        /// <summary>
        /// Gets or sets the number of photos.
        /// </summary>
        /// <value>
        /// The number of photos.
        /// </value>
        public int NumOfPhotos
        {
            get
            {
                return getPhotosNum();
            }
            set
            {
                numOfPhotos = value;
            }
        }
    }
}
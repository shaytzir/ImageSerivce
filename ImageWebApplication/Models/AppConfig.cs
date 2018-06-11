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
            this.objLock = new object();
            this.InitDone = false;
            this.client = WebClient.Instance;
            this.Handlers = new ObservableCollection<string>();
            //when the client recieves informtaion from the server call the handle function
            client.Comm.InfoFromServer += HandleServerCommands;
        }

        public void AskToRemoveHandler(string jsonCommandInfo)
        {
            this.client.Comm.SendCommand(jsonCommandInfo);
            WaitWithBlock();
        }

        public void WaitWithBlock()
        {
            lock (this.objLock)
            {
                Monitor.Wait(this.objLock);
            }
            return;
        }

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
               //new Task(() =>
               //{
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
              // }).Start();
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
                ///Application.Current.Dispatcher.Invoke(new Action(() =>
                //{
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
                this.InitDone = true;
                this.PulseTheBlocking();
            }
            catch (Exception e)
            {
                return;
            }
        }

        private int getPhotosNum()
        {
            int NumOfPhotos = 0;
                string path = Path.Combine(this.m_OutputDir, "Thumbnails");
                if (Directory.Exists(path))
                {
                    NumOfPhotos = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).Count();
                }
            return NumOfPhotos;
            
        }


        private string m_OutputDir;

        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>
        /// The output dir.
        /// </value>
        public string OutputDir
        {
            get
            {
                return m_OutputDir;
            }
            set
            {
                m_OutputDir = value;
            }
        }

        private string m_LogName;

        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>
        /// The name of the log.
        /// </value>
        public string LogName
        {
            get
            {
                return m_LogName;
            }
            set
            {
                m_LogName = value;
            }
        }

        private string m_SourceName;

        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        public string SourceName
        {
            get
            {
                return m_SourceName;
            }
            set
            {
                m_SourceName = value;
            }
        }

        private string m_ThumbnailSize;

        /// <summary>
        /// Gets or sets the size of the thumbnail.
        /// </summary>
        /// <value>
        /// The size of the thumbnail.
        /// </value>
        public string ThumbnailSize
        {
            get
            {
                return m_ThumbnailSize;
            }
            set
            {
                m_ThumbnailSize = value;
            }
        }

        public ObservableCollection<string> Handlers { get; set; }

        private string handlerToRemove;

        /// <summary>
        /// Gets or sets the handlers to remove.
        /// </summary>
        /// <value>
        /// The handlers to remove.
        /// </value>
        public string _HandlersToRemove
        {
            get
            {
                return this.handlerToRemove;
            }
            set
            {
                this.handlerToRemove = value;
            }
        }

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
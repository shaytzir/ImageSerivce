using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ImageWebApplication.Communication;
using System.Windows;

namespace ImageWebApplication.Models
{
    public class AppConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private WebClient client;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AppConfig()
        {
            this.Handlers = new ObservableCollection<string>();
            this.client = WebClient.Instance;
            //when the client recieves informtaion from the server call the handle function
            client.Comm.InfoFromServer += HandleServerCommands;
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
                //Application.Current.Dispatcher.Invoke(new Action(() =>
                //{
                string handlerToRemove = (string)json["Handler"];
                Handlers.Remove(handlerToRemove);
                //}));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                // }));
            }
            catch (Exception e)
            {
                return;
            }
        }
        /// <summary>
        /// Called when  a property changed.
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
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
                this.NotifyPropertyChanged("OutputDir");
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
                this.NotifyPropertyChanged("LogName");
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
                this.NotifyPropertyChanged("SourceName");
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
                this.NotifyPropertyChanged("ThumbnailSize");
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
                OnPropertyChanged("_HandlersToRemove");
            }
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
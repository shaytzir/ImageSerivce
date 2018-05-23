using System.Collections.ObjectModel;
using System.ComponentModel;
using Infrastructure.Event;
using Infrastructure;
using ImageServiceGUI.Communication;
using System;
using Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace ImageServiceGUI.Model
{
    public class SettingModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        private GuiClient guiClient;


        public SettingModel()
        {
            this.Handlers = new ObservableCollection<string>();
            this.guiClient = GuiClient.Instance;

            guiClient.Comm.InfoFromServer += HandleServerCommands;
        }

        private void HandleServerCommands(object sender, string msg)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(msg);
            int commandID = (int)json["CommandID"];
            if (commandID == (int)CommandEnum.GetConfigCommand)
            {
                UpdateConfig(msg);   
            } else if (commandID == (int)CommandEnum.CloseCommand)
            {
                removeHandler(msg);
            }

        }

        public void removeHandler(string msg)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(msg);
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    string handlerToRemove = (string)json["Handler"];
                    Handlers.Remove(handlerToRemove);
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void UpdateConfig(string msg)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(msg);
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
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
                }));
            }
            catch (Exception e)
            {

            }
        }
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        private string m_OutputDir;
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
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }



        public void SendCommand(CommandRecievedEventArgs message)
        {
      //      this.tcpClient.SendCommand(message);
        }
    }
}

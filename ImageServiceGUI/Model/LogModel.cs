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
using System.Collections.Generic;
using ImageService.Logging.Modal;

namespace ImageServiceGUI.Model
{
    class LogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private GuiClient guiClient;
        public ObservableCollection<MessageRecievedEventArgs> Logs { get; set; }
        private JArray m_LogList;
        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>
        /// The name of the log.
        /// </value>
        public JArray LogList
        {
            get
            {
                return m_LogList;
            }
            set
            {
                m_LogList = value;
                this.NotifyPropertyChanged("LogName");
            }
        }
        public LogModel()
        {
            this.Logs = new ObservableCollection<MessageRecievedEventArgs>();
            this.m_TypeList = new ObservableCollection<CommandEnum>();
            this.guiClient = GuiClient.Instance;
            //when the client recieves informtaion from the server call the handle function
            guiClient.Comm.InfoFromServer += HandleServerCommands;
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
            if (commandID == (int)CommandEnum.LogCommand)
            {
                UpdateLog(commandFromSrv);
                //if it closed a directory
            }
        }
        /// <summary>
        /// Updates the log.
        /// </summary>
        /// <param name="CommandFromSrv">The MSG.</param>
        private void UpdateLog(string CommandFromSrv)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(CommandFromSrv);
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    //List<MessageRecievedEventArgs> LogList = (List<MessageRecievedEventArgs>)JsonConvert.DeserializeObject((JArray)json["LogList"]);
                    this.LogList = (JArray)JToken.FromObject(json["LogList"]);
                    ///try to make tow different list of type and message
                    ///for now, xaml gets LogList from the VM
                    for (int i = 0; i < LogList.Count; i++)
                    {
                        Logs.Add(new MessageRecievedEventArgs() { Type = (int)LogList[i]["Status"], Message = (string)LogList[i]["Message"] });
                        m_TypeList.Add((CommandEnum)((int)LogList[i]["Status"]));
                    }
                }));
            }
            catch (Exception e)
            {

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
        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private ObservableCollection<CommandEnum> m_TypeList;
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>
        /// The output dir.
        /// </value>
        public ObservableCollection<CommandEnum> TypeList
        {
            get
            {
                return m_TypeList;
            }
            set
            {
                m_TypeList = value;
                this.NotifyPropertyChanged("TypeList");
            }
        }
    }
}

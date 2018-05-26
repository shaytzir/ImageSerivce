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
      //  private ObservableCollection<MessageRecievedEventArgs> _Logs;
        private ObservableCollection<MessageRecievedEventArgs> _Logs;


        public ObservableCollection<MessageRecievedEventArgs> Logs
        {
            get { return this._Logs; }
            set
            {
                this._Logs = value;
                this.NotifyPropertyChanged("Logs");
            }
        }
        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>
        /// The name of the log.
        /// </value>
        public JArray LogList { get; set; }

        public ObservableCollection<MessageRecievedEventArgs> mylist;
        public LogModel()
        {
            this.Logs = new ObservableCollection<MessageRecievedEventArgs>();
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
                UpdateLog((string)json["LogList"]);
                //if it closed a directory
            }
        }
        /// <summary>
        /// Updates the log.
        /// </summary>
        /// <param name="CommandFromSrv">The MSG.</param>
        private void UpdateLog(string CommandFromSrv)
        {
            List<MessageRecievedEventArgs> LogList = JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(CommandFromSrv);
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                   // JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(json["Loglist"]);
                   // this.LogList = (JArray)JToken.FromObject(json["LogList"]);
                    ///try to make tow different list of type and message
                    //////for now, xaml gets Logs from the VM
                for (int i = 0; i < LogList.Count; i++)
                {
                    //Logs.Add(new MessageRecievedEventArgs() { Type = (int)LogList[i]["Status"], Message = (string)LogList[i]["Message"] });
                    Logs.Insert(0, new MessageRecievedEventArgs() { Status = LogList[i].Status, Message = (string)LogList[i].Message });
                    }
                }));
            }
            catch (Exception e)
            {

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

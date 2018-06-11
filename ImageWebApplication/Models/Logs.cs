using Infrastructure.Modal;
using ImageWebApplication.Communication;
using Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace ImageWebApplication.Models
{ 
    
    public class Logs
    {
        private WebClient client;
        public ObservableCollection<LogEntry> LogList;
        public Logs()
        {
            //this.objLock = new object();
            this.client = WebClient.Instance;
            client.Comm.InfoFromServer += HandleServerCommands;
            this.LogList = new ObservableCollection<LogEntry>();
            this._Logs = new ObservableCollection<LogEntry>();
            return;
        }

        private void HandleServerCommands(object sender, string commandFromSrv)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(commandFromSrv);
            int commandID = (int)json["CommandID"];
            //if it send the configuration info
            if (commandID == (int)CommandEnum.LogCommand)
            {
                UpdateLog((string)json["LogList"]);
            }

        }

        /// <summary>
        /// Updates the log.
        /// </summary>
        /// <param name="CommandFromSrv">The MSG.</param>
        private void UpdateLog(string CommandFromSrv)
        {
            this.LogList = JsonConvert.DeserializeObject<ObservableCollection<LogEntry>>(CommandFromSrv);
            try
            {
                    //creat log list to output.
                    //this._Logs = new List<LogEntry>();
                    for (int i = 0; i < LogList.Count; i++)
                    {
                        _Logs.Insert(0, new LogEntry() { Status = LogList[i].Status.ToString(), Message = (string)LogList[i].Message });
                    }
            }
            catch (Exception e)
            {
                return;
            }
        }
        public ObservableCollection<LogEntry> _Logs { get; set; }
        /*[Required]
        [DataType(DataType.Text)]
        [Display(Name = "_Logs")]
        public List<LogEntry> _Logs { get; set; }*/

        /*[HttpPost]
        public void GetLogInfo()
        {
            ObservableCollection<LogEntry> logInfo = new ObservableCollection<LogEntry>();
            foreach(LogEntry log in _Logs)
            {
                if (log.Status == "INFO")
                {
                    logInfo.Add(log);
                }
            }
            this._Logs = logInfo;
        }*/

        public List<LogEntry> GetLogInfo(string filter)
        {
            List<LogEntry> data = new List<LogEntry>();
            foreach (LogEntry log in _Logs)
            {
                switch (filter)
                {
                    case ("info"):
                        if (log.Status == "0")
                        {
                            data.Add(log);
                            //data["Status"] = "INFO";
                            //data["Message"] = log.Message;
                        }
                        break;
                    case (null):
                        data.Add(log);
                        //data["Status"] = log.Status;
                        //data["Message"] = log.Message;
                        break;
                    case ("warning"):
                        if (log.Status == "1")
                        {
                            data.Add(log);
                            //data["Status"] = "WARNING";
                            //data["Message"] = log.Message;
                        }
                        break;
                    case ("fail"):
                        if (log.Status == "2")
                        {
                            data.Add(log);
                            //data["Status"] = "FAIL";
                            //data["Message"] = log.Message;
                        }
                        break;
                }
            }
            return data;
        }
    }
}
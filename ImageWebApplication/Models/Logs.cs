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
        public List<LogEntry> data { get; set; }
        public Logs()
        {
            this.client = WebClient.Instance;
            client.Comm.InfoFromServer += HandleServerCommands;
            this.LogList = new ObservableCollection<LogEntry>();
            this._Logs = new ObservableCollection<LogEntry>();
            this.data = new List<LogEntry>();
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
                for (int i = 0; i < LogList.Count; i++)
                {
                    LogEntry log = null;
                    switch (LogList[i].Status)
                    {
                        case "0":
                            log = new LogEntry() { Status = "INFO", Message = (string)LogList[i].Message };
                            break;
                        case "1":
                            log = new LogEntry() { Status = "WARNNING", Message = (string)LogList[i].Message };
                            break;
                        case "2":
                            log = new LogEntry() { Status = "FAIL", Message = (string)LogList[i].Message };
                            break;
                    }
                    _Logs.Insert(0, log);
                  

                }
            }
            catch (Exception e)
            {
                return;
            }
        }
        public ObservableCollection<LogEntry> _Logs { get; set; }

        public void RefreshLogs()
        {
            for (int i = 0; i < LogList.Count; i++)
            {
                LogEntry log = null;
                switch (LogList[i].Status)
                {
                    case "0":
                        log = new LogEntry() { Status = "INFO", Message = (string)LogList[i].Message };
                        break;
                    case "1":
                        log = new LogEntry() { Status = "WARNNING", Message = (string)LogList[i].Message };
                        break;
                    case "2":
                        log = new LogEntry() { Status = "FAIL", Message = (string)LogList[i].Message };
                        break;
                }
                data.Add(log);
            }
        }
    
       
        public List<LogEntry> FilterLogs(string filter)
        {
            string filterlower = filter.ToLower();
            data = new List<LogEntry>();
            foreach (LogEntry log in _Logs)
            {
                switch (filterlower)
                {
                    case ("info"):
                        if (log.Status == "INFO")
                        {
                            data.Add(log);
                        }
                        break;
                    case (""):
                        data.Add(log);
                        break;
                    case ("warning"):
                        if (log.Status == "WARNNING")
                        {
                            data.Add(log);
                        }
                        break;
                    case ("fail"):
                        if (log.Status == "FAIL")
                        {
                            data.Add(log);
                        }
                        break;
                }
            }
            return data;
        }
    }
}
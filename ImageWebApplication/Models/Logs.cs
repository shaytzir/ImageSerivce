using ImageService.Logging.Modal;
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

namespace ImageWebApplication.Models
{ 
    
    public class Logs
    {
        //private ObservableCollection<MessageRecievedEventArgs> _Logs;
        private WebClient client;
        private object objLock;

        public Logs()
        {
            this.objLock = new object();
            this.client = WebClient.Instance;
            client.Comm.InfoFromServer += HandleServerCommands;
          /*  lock (this.objLock)
            {
                Monitor.Wait(this.objLock);
            }*/
            return;
        }


        private void WaitWithBlock()
        {
            lock (this.objLock)
            {
                Monitor.Wait(this.objLock);
            }
            return;
        }

        private void PulseTheBlocking()
        {
            lock (this.objLock)
            {
                Monitor.Pulse(this.objLock);
            }
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
            List<MessageRecievedEventArgs> LogList = JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(CommandFromSrv);
            try
            {
                    //creat log list to output.
                    this._Logs = new List<MessageRecievedEventArgs>();
                    for (int i = 0; i < LogList.Count; i++)
                    {
                        _Logs.Insert(0, new MessageRecievedEventArgs() { Status = LogList[i].Status, Message = (string)LogList[i].Message });
                    }
            }
            catch (Exception e)
            {
                return;
            } /*finally
            {
                lock (this.objLock)
                {
                    Monitor.Pulse(this.objLock);
                }
            }*/
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public List<MessageRecievedEventArgs> _Logs { get; set; }
    }
}
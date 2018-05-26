using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    /// <summary>
    /// defines a message
    /// </summary>
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageTypeEnum _Status;
        /// <summary>
        /// setter/getter for the status of the message - info/fail/warning
        /// </summary>
        public MessageTypeEnum Status {
            get
            {
                return _Status;
            }

            set
            {
                this._Status = value;
                //RaiseProperChanged();
            }
        }
      //  public int Type { get; set; }

        public string _Message;
        /// <summary>
        /// setter/getter for the message infos
        /// </summary>
        public string Message {
            get
            {
                return _Message;
            }

            set
            {
                this._Message = value;
               // RaiseProperChanged();
            }
        }
   /*     public event PropertyChangedEventHandler PropertyChanged;

        private void RaiseProperChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }*/
    }
}

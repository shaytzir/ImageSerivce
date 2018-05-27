using System;

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
            }
        }

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
            }
        }
    }
}

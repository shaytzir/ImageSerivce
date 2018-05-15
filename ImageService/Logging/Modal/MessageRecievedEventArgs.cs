using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    /// <summary>
    /// defines a message
    /// </summary>
    public class MessageRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// setter/getter for the status of the message - info/fail/warning
        /// </summary>
        public MessageTypeEnum Status { get; set; }

        /// <summary>
        /// setter/getter for the message infos
        /// </summary>
        public string Message { get; set; }
    }
}

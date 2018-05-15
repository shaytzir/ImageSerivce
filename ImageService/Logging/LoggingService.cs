
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// a class implemets the ILoggindService - logs messages
    /// </summary>
    public class LoggingService : ILoggingService
    {
        /// <summary>
        /// event when a message is recieved
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// the logging service invokes it's event for all subscribers to operate
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="type">type of the message - INFO\FAILE</param>
        public void Log(string message, MessageTypeEnum type)
        {
            //create relevant occurence of MessageRecieveArgs containing the message and the type
            MessageRecievedEventArgs messageRecievedEventArgs = new MessageRecievedEventArgs()
            {
                Status = type,
                Message = message
            };
            //notify subscribers of the new message recieved
            MessageRecieved?.Invoke(this, messageRecievedEventArgs);
        }
    }
}

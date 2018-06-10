using Infrastructure.Enums;
using Infrastructure.Modal;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Threading;

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
        List<MessageRecievedEventArgs> _LogList;
        private static Mutex m_mutex = new Mutex();

        public List<MessageRecievedEventArgs> LogList
        {
            get
            {
                return this._LogList;
            }
            set
            {
                this._LogList = value;
            }
        }
        public LoggingService() {
            this._LogList = new List<MessageRecievedEventArgs>();
        }
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
            m_mutex.WaitOne();
            this._LogList.Add(messageRecievedEventArgs);
            m_mutex.ReleaseMutex();
            //notify subscribers of the new message recieved
            MessageRecieved?.Invoke(this, messageRecievedEventArgs);
        }
    }
}

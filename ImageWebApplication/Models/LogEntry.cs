using Infrastructure.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageWebApplication.Models
{
    /// <summary>
    /// this class represents an entry of a lof
    /// </summary>
    public class LogEntry
    {
        public string _Status;

        /// <summary>
        /// setter/getter for the status of the message - info/fail/warning
        /// </summary>
        public string Status
        {
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
        public string Message
        {
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

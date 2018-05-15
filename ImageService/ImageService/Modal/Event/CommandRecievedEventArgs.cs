using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    /// <summary>
    /// command recieved args
    /// </summary>
    public class CommandRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// The Command ID
        /// </summary>
        public int CommandID { get; set; }

        /// <summary>
        /// info for further on executions
        /// </summary>
        public string[] Args { get; set; }
        /// <summary>
        /// The Request Directory
        /// </summary>
        public string RequestDirPath { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id">command id</param>
        /// <param name="args">info for the command</param>
        /// <param name="path">relevant directory path</param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}

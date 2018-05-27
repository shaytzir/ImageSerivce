using System;

namespace Infrastructure.Event
{
    /// <summary>
    /// closing directory args
    /// </summary>
    public class DirectoryCloseEventArgs : EventArgs
    {
        /// <summary>
        /// which directory to close
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// The Message That goes to the logger
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dirPath"><dir to close/param>
        /// <param name="message">message to log</param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            //Setting the Directory Name
            DirectoryPath = dirPath;
            //Storing the String
            Message = message;
        }
    }
}

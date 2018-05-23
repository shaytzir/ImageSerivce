using ImageService.Modal;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// interface for the directory handler
    /// </summary>
    public interface IDirectoryHandler
    {
        /// <summary>
        /// The Event That Notifies that the Directory is being closed
        /// </summary>
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        /// <summary>
        /// The Function Recieves the directory to Handle
        /// </summary>
        /// <param name="dirPath"> the path of the current direcotry handler</param>
        void StartHandleDirectory(string dirPath);

        /// <summary>
        /// The Event that will be activated upon new Command
        /// </summary>
        /// <param name="sender">the sender which this func subscribed to</param>
        /// <param name="e">info about the command that was recieved</param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);
    }
}

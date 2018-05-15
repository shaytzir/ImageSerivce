using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Modal;
using System.Collections.Generic;

namespace ImageService.Controller
{
    /// <summary>
    /// class of image controller
    /// </summary>
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;
        private Dictionary<int, ICommand> commands;

        /// <summary>
        /// constuctor
        /// </summary>
        /// <param name="modal">image service modal to contain</param>
        public ImageController(IImageServiceModal modal)
        {
            // Storing the Modal Of The System
            m_modal = modal;
            //creating the right dictionary for the possible commands
            commands = new Dictionary<int, ICommand>()
            {
                // For Now will contain NEW_FILE_COMMAND
                {(int)CommandEnum.NewFileCommand , new NewFileCommand(m_modal) }
            };
        }

        /// <summary>
        /// execution func
        /// </summary>
        /// <param name="commandID">specific command id - extracting from the dictionary</param>
        /// <param name="args">info for execution</param>
        /// <param name="resultSuccesful">true/false - success/failure</param>
        /// <returns>new file path/error message - string</returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            //calls the specific command from the dictionary to execute itself with the proper args
            return commands[commandID].Execute(args, out resultSuccesful);
        }
    }
}

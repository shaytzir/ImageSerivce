using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// a class of the new File command
    /// </summary>
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        /// <summary>
        /// a constuctor
        /// </summary>
        /// <param name="modal">the service modal to hold</param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;
        }

        /// <summary>
        /// executing the new file command - adding a file using the image service
        /// modal.
        /// </summary>
        /// <param name="args">info for execution</param>
        /// <param name="result">false-failure, true-success</param>
        /// <returns>if result=true returns the new path of the file, otherwise an error message</returns>
        public string Execute(string[] args, out bool result)
        {
            // assuming args[0] holds the path of the file
            return m_modal.AddFile(args[0], out result);
        }
    }
}

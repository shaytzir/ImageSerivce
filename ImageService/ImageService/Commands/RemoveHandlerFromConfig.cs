﻿using ImageService.Modal;

namespace ImageService.Commands
{
    public class RemoveHandlerFromConfig : ICommand
    {
        private IImageServiceModal m_modal;

        /// <summary>
        /// a constuctor
        /// </summary>
        /// <param name="modal">the service modal to hold</param>
        public RemoveHandlerFromConfig(IImageServiceModal modal)
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
            return m_modal.RemoveHandlerFromConfig(args[0], out result);
        }
    }
}
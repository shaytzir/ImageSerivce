using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// interface for the image controller
    /// </summary>
    public interface IImageController
    {
        /// <summary>
        /// executing the command func
        /// </summary>
        /// <param name="commandID">a specific command id to know which one to execute</param>
        /// <param name="args">info for execution (including th file path)</param>
        /// <param name="result">success/failure = true/false</param>
        /// <returns>new file path/error message</returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);
    }
}

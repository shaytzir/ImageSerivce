using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// an interface for a command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// function to execute the implemented command
        /// </summary>
        /// <param name="args">info for execution</param>
        /// <returns></returns>
        void Execute(string[] args);
    }
}

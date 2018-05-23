using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Enums
{
    /// <summary>
    /// enum for the possible commands
    /// </summary>
    public enum CommandEnum : int
    {
        NewFileCommand,
        GetConfigCommand,
        LogCommand,
        CloseCommand
    }
}
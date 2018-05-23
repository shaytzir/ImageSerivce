
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication
{
    /// <summary>
    /// client handler interface
    /// </summary>
    public interface IClientHandler
    {

        //void HandleClient(TcpClient client,ObservableCollection<TcpClient> clients);
        void HandleClient();
        void SendCommand(string commandToTcpClient);
        BinaryWriter GetWriter();
        void Close();
        Mutex M_mutex { get; set; }
    }
}
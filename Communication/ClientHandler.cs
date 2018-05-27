using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication
{
    public class ClientHandler : IClientHandler
    {
        public event EventHandler<string> GotCommandFromGui;
        public event EventHandler HandlerClosed;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private bool connected;
        private static Mutex m_mutex = new Mutex();


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="acceptedOnServer">the tcp client that was accepted on the tcpServer</param>
        public ClientHandler(TcpClient acceptedOnServer)
        {
            this.client = acceptedOnServer;
            this.stream = client.GetStream();
            this.reader = new BinaryReader(stream, Encoding.ASCII);
            this.writer = new BinaryWriter(stream, Encoding.ASCII);
            connected = true;
        }

        /// <summary>
        /// Handles the client - trying to read messages from client.
        /// </summary>
        public void HandleClient()
        {
            new Task(() =>
            {
                while (connected)
                {
                    try
                    {
                        string input = reader.ReadString();
                        GotCommandFromGui?.Invoke(this, input);
                        
                    } catch (Exception e)
                    {
                        Close();
                    }
                }
            }).Start();


        }

        /// <summary>
        /// Sends the command to the client.
        /// </summary>
        /// <param name="commandToTcpClient">The command to TCP client.</param>
        public void SendCommand(string commandToTcpClient)
        {
            new Task(() => {
                try
                {
                    //locks the writer
                    m_mutex.WaitOne();
                    this.writer.Write(commandToTcpClient);
                    m_mutex.ReleaseMutex();
                }
                catch (Exception e)
                {
                    Console.WriteLine("DEBUG in SendCommand in TcpTImeClient");
                }
            }).Start();

        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            connected = false;
            this.reader.Close();
            this.writer.Close();
            this.stream.Close();
            this.client.Close();
            this.HandlerClosed?.Invoke(this, null);
        }
    }
}

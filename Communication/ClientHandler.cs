using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;

namespace Communication
{
    public class ClientHandler : IClientHandler
    {
        public event EventHandler<string> GotCommandFromGui;
        //private CancellationTokenSource tokenSource = new CancellationTokenSource();
        // private CancellationToken tokenSource;
        public event EventHandler HandlerClosed;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private bool connected;

        public BinaryWriter Writer => this.writer;
        public void Close()
        {
            connected = false;
            this.reader.Close();
            this.writer.Close();
            this.stream.Close();
            this.client.Close();
            this.HandlerClosed?.Invoke(this,null);
        }

        public ClientHandler(TcpClient acceptedOnServer)
        {
            this.client = acceptedOnServer;
            this.stream = client.GetStream();
            this.reader = new BinaryReader(stream, Encoding.ASCII);
            this.writer = new BinaryWriter(stream, Encoding.ASCII);
            connected = true;
        }

        public void HandleClient()
        {
            new Task(() =>
            {

                while (connected)
                {
                    try
                    {
                        bool result;
                        string input = reader.ReadString();
                        //DebugClientHandler?.Invoke(this, input);
                        if (input != "") 
                        {
                            string debug = input;
                            GotCommandFromGui?.Invoke(this, input);
                        }
                    } catch (Exception e)
                    {
                        Close();

                       // DebugClientHandler?.Invoke(this, "exception in reviece debug client handler " + e);
                        //this.tokenSource.Cancel();
                        //  m_logger.Log("Server failed due to: " + e.Message, MessageTypeEnum.FAIL);
                    }
                }
                //}, this.tokenSource.Token).Start();
            }).Start();


        }

        public void SendCommand(string commandToTcpClient)
        {

            new Task(() => {
                try
                {
                    this.writer.Write(commandToTcpClient);
                }
                catch (Exception e)
                {
                    Console.WriteLine("DEBUG in SendCommand in TcpTImeClient");
                }
            }).Start();

        }
        public BinaryWriter GetWriter()
        {
            return this.writer;
        }
    }
}

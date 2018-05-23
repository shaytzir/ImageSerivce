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
        public static event EventHandler<string> DebugClientHandler;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        // private CancellationToken tokenSource;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        public Mutex M_mutex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BinaryWriter Writer => this.writer;
        public void Close()
        {
            throw new NotImplementedException();
        }

        public ClientHandler(TcpClient acceptedOnServer)
        {
            this.client = acceptedOnServer;
            this.stream = client.GetStream();
            this.reader = new BinaryReader(stream, Encoding.ASCII);
            this.writer = new BinaryWriter(stream, Encoding.ASCII);
        }

        public void HandleClient()
        {
            new Task(() =>
            {

                while (true)
                {
                    try
                    {
                        bool result;
                        string input = reader.ReadString();
                        DebugClientHandler?.Invoke(this, input);
                        if (input != "") 
                        {
                            string debug = input;
                            GotCommandFromGui?.Invoke(this, input);
                        }
                    } catch (Exception e)
                    {
                        DebugClientHandler?.Invoke(this, "exception in reviece debug client handler " + e);
                        this.tokenSource.Cancel();
                        //  m_logger.Log("Server failed due to: " + e.Message, MessageTypeEnum.FAIL);
                    }
                }
            }, this.tokenSource.Token).Start();

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

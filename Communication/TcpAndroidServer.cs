using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication
{
    public class TcpAndroidServer
    {
        TcpListener listener;
        private List<ITcpAndroidHandler> ch;
        private static Mutex m_mutex = new Mutex();
        private bool running;
        private string IP;
        private int port;
        private string[] paths;

        /// <summary>
        /// Initializes a new instance of the TcpTimeServer class.
        /// </summary>
        public TcpAndroidServer(string[] handlers)
        {
            this.IP = "127.0.0.1";
            this.port = 8007;
            this.ch = new List<ITcpAndroidHandler>();
            this.running = false;
            this.paths = handlers;
        }


        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.IP), port);
            this.listener = new TcpListener(ep);
            this.listener.Start();
            running = true;
            Task task = new Task(() =>
            {
                //as long as the server is running keep listening for new clients
                while (running)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        ITcpAndroidHandler newHandler = new TcpAndroidHandler(client, paths[1]);
                        m_mutex.WaitOne();
                        //add it to the list
                        ch.Add(newHandler);
                        m_mutex.ReleaseMutex();
                        newHandler.HandleClient();
                    }
                    catch (SocketException e)
                    {
                        this.running = false;
                        Close();
                    }
                }
            });
            task.Start();

        }


        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            this.running = false;
            this.listener.Stop();
            Task task = new Task(() =>
            {
                m_mutex.WaitOne();
                foreach (ITcpAndroidHandler handler in ch)
                {
                    try
                    {
                        handler.Close();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                m_mutex.ReleaseMutex();
            });
            task.Start();
        }
    }
}

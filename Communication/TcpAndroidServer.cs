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
        public event EventHandler<IClientHandler> NewClientConnected;
        public event EventHandler<string> PassInfoFromClientHandlerToServer;
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
                        ITcpAndroidHandler newHandler = new TcpAndroidHandler(client, paths[0]);
                        m_mutex.WaitOne();
                        //add it to the list
                        ch.Add(newHandler);
                        m_mutex.ReleaseMutex();
                        //let the main server know a new client connected
                        //NewClientConnected?.Invoke(this, newHandler);
                        //start handeling the new client
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
        /// Removes the handler from list.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
     /*   private void RemoveHandlerFromList(object sender, EventArgs e)
        {
            IClientHandler handlerToRemove = (IClientHandler)sender;
            m_mutex.WaitOne();
            this.ch.Remove(handlerToRemove);
            m_mutex.ReleaseMutex();
        }*/

        /// <summary>
        /// Sends a message from the server to all clients.
        /// </summary>
        /// <param name="info">The information.</param>
      /*  public void SendToAllClients(string info)
        {
            Task task = new Task(() =>
            {
                m_mutex.WaitOne();
                foreach (IClientHandler handler in ch)
                {
                    try
                    {
                        handler.SendCommand(info);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                m_mutex.ReleaseMutex();
            });
            task.Start();

        }*/

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
                foreach (IClientHandler handler in ch)
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

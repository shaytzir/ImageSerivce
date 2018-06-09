using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageWebApplication.Communication
{
    public class WebClient
    {
        private static WebClient instance;
        private bool connected;
        public TcpTimeClient Comm { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static WebClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WebClient();
                }
                return instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiClient"/> class.
        /// </summary>
        public WebClient()
        {
            //debug
            // should set the ip and port here and then create new tcpClient
            this.Comm = new TcpTimeClient();
            Comm.Start();
            Connected = Comm.Connected;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this "GuiClient" is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected
        {
            get
            {
                return this.connected;
            }
            set { this.connected = value; }
        }
    }
}
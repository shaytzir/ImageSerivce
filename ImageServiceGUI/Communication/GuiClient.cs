using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    class GuiClient
    {
        private static GuiClient instance;
        private bool connected;
        public TcpTimeClient Comm { get; set; }

        public static GuiClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GuiClient();
                }
                return instance;
            }
        }


        public GuiClient()
        {
            //debug
            // should set the ip and port here and then create new tcpClient
            this.Comm = new TcpTimeClient();
            Comm.Start();
            Connected = Comm.Connected;

        }

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

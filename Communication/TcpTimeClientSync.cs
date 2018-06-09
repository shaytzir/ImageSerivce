using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Infrastructure;

using System.Threading;

public sealed class TcpTimeClientSync
{
    public event EventHandler<string> InfoFromServer;
    TcpClient client;
    NetworkStream ns;
    BinaryReader reader;
    BinaryWriter writer;
    bool connected;
    private string ip;
    private int port;
    private static Mutex m_mutex = new Mutex();


    private Settings _SettingObj;

    public TcpTimeClientSync()
    {
        connected = false;
        this.ip = "127.0.0.1";
        this.port = 8006;
    }

    /// <summary>
    /// starts the client -  tries to connect
    /// </summary>
    public void Start()
    {
        try
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            this.client = new TcpClient();
            client.Connect(ep);
            this.ns = this.client.GetStream();
            this.reader = new BinaryReader(this.ns);
            this.writer = new BinaryWriter(this.ns);
            connected = true;
            //try commands from the server
     //       RecieveCommand();

        }
        catch (SocketException e)
        {
            connected = false;
        }


    }

    /// <summary>
    /// Gets a value indicating whether this TcpTimeClientis connected.
    /// </summary>
    /// <value>
    ///   true if connected; otherwise,false.
    /// </value>
    public bool Connected
    {
        get
        {
            return this.connected;
        }
    }

    /// <summary>
    /// Sends the command.
    /// </summary>
    /// <param name="jsonCommandInfo">The json command information.</param>
    public void SendCommand(string jsonCommandInfo)
    {
       // new Task(() => {
            try
            {
                m_mutex.WaitOne();
                this.writer.Write(jsonCommandInfo);
                m_mutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                Console.WriteLine("DEBUG in SendCommand in TcpTImeClient");
            }
       // }).Start();

    }

    /// <summary>
    /// reads commands from the server.
    /// </summary>
    public string RecieveCommand()
    {
        string response = "";
      //  new Task(() =>
      //  {
      //as long as this client is connected
      // while (connected)
      //{
        try
                {
                     response = this.reader.ReadString();
                    //InfoFromServer?.Invoke(this, response);
                }
                catch (Exception e)
                {
                    this.connected = false;
                }
        return response;
          //  }
      //  }).Start();
    }

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public void Close()
    {
        this.connected = false;
        this.client.Close();
    }
}
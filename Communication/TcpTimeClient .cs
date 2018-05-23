using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Infrastructure;
using System.Diagnostics;

public sealed class TcpTimeClient
{
    public event EventHandler<string> InfoFromServer;
    TcpClient client;
    NetworkStream ns;
    BinaryReader reader;
    BinaryWriter writer;
    bool connected;
    private string ip;
    private int port;
    //private static TcpTimeClient instance;

    private Settings _SettingObj;

    public TcpTimeClient()
    {
        connected = false;
        this.ip = "127.0.0.1";
        this.port = 8006;
    }

    public Settings SettingObj
    {
        get { return _SettingObj; }
        set { _SettingObj = value; }
    }
   // private static readonly TcpTimeClient instance = new TcpTimeClient();

   /* private TcpTimeClient() {
       // this._SettingObj = new Settings();
        Start();
    }*/


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
            RecieveCommand();

        }
        catch (SocketException e)
        {
            connected = false;
        }

        
    }

    public bool Connected
    {
        get
        {
            return this.connected;
        }
    }

    private void ReadString()
    {
        byte[] bytes = new byte[1024];
        int bytesRead = this.ns.Read(bytes, 0, bytes.Length);
        string str = Encoding.ASCII.GetString(bytes, 0, bytesRead);
        this.SettingObj = new Settings();
        JObject fromJson = JObject.Parse(str);
        SettingObj.LogName = (string) fromJson["LogName"];
        SettingObj.OutputDir = (string)fromJson["OutputDir"];
        SettingObj.SourceName = (string)fromJson["SourceName"];
        SettingObj.ThumbSize = (string)fromJson["ThumbSize"];
        SettingObj.Handlers = (string)fromJson["Handlers"];
    }

    /* public void SendCommand(string command)
     {
         using (NetworkStream stream = client.GetStream())
         using (BinaryWriter writer = new BinaryWriter(stream))
         {
             writer.Write(Encoding.ASCII.GetBytes(command), 0, command.Length);
         }
     }*/



    /*
     * should add mutxes? + using the instance of the server?????????? DEBUG
     * */

    public void SendCommand(string jsonCommandInfo)
    {

        new Task(() => {
            try
            {
                this.writer.Write(jsonCommandInfo);
            } catch (Exception e)
            {
                Console.WriteLine("DEBUG in SendCommand in TcpTImeClient");
            }
        }).Start();

    }

    public void RecieveCommand()
    {

        new Task(() =>
        {
            while (connected)
            {
                try
                {
                    string response = this.reader.ReadString();
                    
                    if (response != "")
                    {
                        //CommandInfo responseObj = CommandInfo.ParseJSON(response);
                        //InfoFromServer?.Invoke(this, responseObj);
                        InfoFromServer?.Invoke(this, response);
                    }
                }
                catch (Exception e)
                {


                }
            }
        }).Start();

    }

    public void Close()
    {
        this.client.Close();
    }
}
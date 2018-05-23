
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Event;
using System.Collections.Generic;
using Communication;
//using ImageService.Logging;

public class TcpTimeServer
{
    private const int portNum = 13;
    //TcpClient client;
    TcpListener listener;
    private List<IClientHandler> ch;
    private ObservableCollection<TcpClient> clients = new ObservableCollection<TcpClient>();
    private static Mutex m_mutex = new Mutex();
    public event EventHandler<IClientHandler> NewClientConnected;
    public event EventHandler<string> PassInfoFromClientHandlerToServer;
    private bool running;

    public TcpTimeServer()
    {
       
        this.ch = new List<IClientHandler>();
        this.running = false;
    }

    public void Start()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8006);
        this.listener = new TcpListener(ep);
        this.listener.Start();
        running = true;
        Task task = new Task(() =>
        {
            while (running)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientHandler newHandler = new ClientHandler(client);
                    newHandler.GotCommandFromGui += this.PassInfoFromClientHandlerToServer;
                    ch.Add(newHandler);
                    NewClientConnected?.Invoke(this, newHandler);
                    newHandler.HandleClient();
                }
                catch (SocketException e)
                {
                }
            }
        });
        task.Start();

    }



    /*public void Send(byte[] settingsObj)
    {
        string str;
        int bytesRead = 0;
        using (NetworkStream stream = client.GetStream())
        using (BinaryReader reader = new BinaryReader(stream))
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(settingsObj, 0, settingsObj.Length);
            do
            {
                byte[] bytes = new byte[1024];
                bytesRead = stream.Read(bytes, 0, bytes.Length);
            //    m_logging.Log("bytesRead", ImageService.Logging.Modal.MessageTypeEnum.INFO);
                str = Encoding.ASCII.GetString(bytes, 0, bytesRead);
            } while (str != "close");
        }
        this.client.Close();
    }*/


    /*public string GetCommand()
    {
        string str;
        int bytesRead = 0;
        using (NetworkStream stream = client.GetStream())
        using (BinaryReader reader = new BinaryReader(stream))
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            byte[] bytes = new byte[1024];
            bytesRead = stream.Read(bytes, 0, bytes.Length);
            str = Encoding.ASCII.GetString(bytes, 0, bytesRead);
        }
        return str;
    }*/

    public void SendToAllClients(string info)
    {
        Task task = new Task(() =>
        {

            foreach (IClientHandler handler in ch)
            {
                try
                {
                    BinaryWriter writer = handler.GetWriter();
                    writer.Write(info);
                    
                } catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        });
        task.Start();

    }

    public void Close()
    {
        //this.client.Close();
        this.listener.Stop();
    }

}
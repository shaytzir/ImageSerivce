using ImageService.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

public class TcpTimeServer
{
    private const int portNum = 13;
    TcpClient client;
    TcpListener listener;
    public void Start(ILoggingService m_logging)
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
        this.listener = new TcpListener(ep);
        this.listener.Start();
        m_logging.Log("Waiting for client connections...", ImageService.Logging.Modal.MessageTypeEnum.INFO);
        this.client = listener.AcceptTcpClient();
        m_logging.Log("Client connected", ImageService.Logging.Modal.MessageTypeEnum.INFO);
    }
    public void Send(byte[] settingsObj, ILoggingService m_logging)
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
                str = Encoding.ASCII.GetString(bytes, 0, bytesRead);
            } while (str != "close");
        }        
        this.client.Close();
    }
    
    public void Close()
    {
        this.client.Close();
        this.listener.Stop();
    }

}
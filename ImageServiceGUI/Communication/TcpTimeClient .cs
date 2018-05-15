using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using ImageServiceGUI.Model;

public sealed class TcpTimeClient
{
    TcpClient client;
    NetworkStream ns;
    private Settings _SettingObj;
    public Settings SettingObj
    {
        get { return _SettingObj; }
        set { _SettingObj = value; }
    }
    private static readonly TcpTimeClient instance = new TcpTimeClient();
    private TcpTimeClient() {
        Start();
    }
    public static TcpTimeClient Instance
    {
        get
        {
            return instance;
        }
    }
    public void Start()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
        try
        {
            this.client = new TcpClient();
            client.Connect(ep);
            Console.WriteLine("You are connected");
            this.ns = this.client.GetStream();
            ReadString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
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
    public void SendCommand(string command)
    {
        using (NetworkStream stream = client.GetStream())
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(Encoding.ASCII.GetBytes(command), 0, command.Length);
        }
    }
    public string GetAllHendlers()
    {
        if (this.SettingObj == null)
        {
            ReadString();

        }
        return this.SettingObj.Handlers;
    }
    public void Close()
    {
        this.client.Close();
    }
}
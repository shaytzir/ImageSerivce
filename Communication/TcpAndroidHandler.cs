using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication
{
    class TcpAndroidHandler : ITcpAndroidHandler
    {
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private bool connected;
        private string path;
        private static Mutex m_mutex = new Mutex();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="acceptedOnServer"> the tcp client we handle</param>
        /// <param name="path">path of the handler to save to</param>
        public TcpAndroidHandler(TcpClient acceptedOnServer, string path)
        {
            this.client = acceptedOnServer;
            this.stream = client.GetStream();
            this.reader = new BinaryReader(stream, Encoding.ASCII);
            this.writer = new BinaryWriter(stream, Encoding.ASCII);
            this.path = path;
            connected = true;
        }

        /// <summary>
        /// handles the client, read information and all bytes of the images
        /// </summary>
        public void HandleClient()
        {
            new Task(() =>
            {
                while (connected)
                {
                    try
                    {
                        //reading from client
                        byte[] bytesArr = new byte[4096];
                        int res = stream.Read(bytesArr, 0, bytesArr.Length);
                        string message = Encoding.ASCII.GetString(bytesArr, 0, res);
                        //spliting into size in bytes and name of file
                        string[] sizeAndName = message.Split(' ');
                        int size = int.Parse(sizeAndName[0]);
                        string name = sizeAndName[1];
                        byte[] confirm = new byte[1];
                        confirm[0] = 1;
                        //letting the client know we got the infortmation
                        stream.Write(confirm, 0, confirm.Length);
                        bytesArr = new byte[size];
                        //reading the bytes of the images
                        int readnumFirst = stream.Read(bytesArr, 0, bytesArr.Length);
                        int numRead = readnumFirst;
                        while (numRead < size)
                        {
                            int num = stream.Read(bytesArr, numRead, bytesArr.Length-numRead);
                            numRead += num;
                        }
                        SaveImageInHandler(bytesArr, name);

                    }
                    catch (Exception e)
                    {
                        Close();
                    }
                }
            }).Start();
        }

        /// <summary>
        /// saving the image into the right directory
        /// </summary>
        /// <param name="bytesArr"><the image bytes/param>
        /// <param name="name">name of the image</param>
        private void SaveImageInHandler(byte[] bytesArr, string name)
        {
            try
            {
                File.WriteAllBytes(path + "\\" + name, bytesArr);
            }
            catch (Exception e)
            {

            }
        }

        public void Close()
        {
            this.client.Close();
            this.connected = false;
            this.reader.Close();
            this.writer.Close();

        }
    }
}

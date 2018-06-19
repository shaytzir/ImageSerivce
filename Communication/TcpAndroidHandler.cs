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

        public TcpAndroidHandler(TcpClient acceptedOnServer, string path)
        {
            this.client = acceptedOnServer;
            this.stream = client.GetStream();
            this.reader = new BinaryReader(stream, Encoding.ASCII);
            this.writer = new BinaryWriter(stream, Encoding.ASCII);
            this.path = path;
            connected = true;
        }

        public void HandleClient()
        {
            new Task(() =>
            {
                while (connected)
                {
                    try
                    {
                        byte[] bytesArr = new byte[4096];
                        int res = stream.Read(bytesArr, 0, bytesArr.Length);
                        string message = Encoding.ASCII.GetString(bytesArr, 0, res);
                        string[] sizeAndName = message.Split(' ');
                        int size = int.Parse(sizeAndName[0]);
                        string name = sizeAndName[1];
                        bytesArr = new byte[size];
                        int readnumFirst = stream.Read(bytesArr, 0, bytesArr.Length);
                        int numRead = readnumFirst;
                        while (numRead < size)
                        {
                            int num = stream.Read(bytesArr, numRead, bytesArr.Length-numRead);
                            numRead += num;
                        }
                        saveImageInHandler(bytesArr, name);

                    }
                    catch (Exception e)
                    {
                        string msg = "oh no  debug";
                        //Close();
                    }
                }
            }).Start();
        }

        private void saveImageInHandler(byte[] bytesArr, string name)
        {
            using (var ms = new MemoryStream(bytesArr))
            {
                // Put the new image in one of our handlers
                try
                {
                    File.WriteAllBytes(path + "\\" + name, bytesArr);
                }
                catch (Exception e)
                {
                    string msg = "oh no i die debug";
                }
            }
        }

        public void transferBytes(byte[] origin, byte[] toCopy, int start)
        {
            for (int i = start; i < origin.Length; i++)
            {
                origin[i] = toCopy[i - start];
            }
        }

        public void byteArrayToImage(byte[] byteArray, string picName)
        {
            //Image image = (Bitmap)((new ImageConverter()).ConvertFrom(byteArray));
            using (var ms = new MemoryStream(byteArray))
            {
                //Image image = Image.FromStream(ms);
                //string imgName = image.
                File.WriteAllBytes(path + "\\" + picName, byteArray);
            }

        }
    }
}

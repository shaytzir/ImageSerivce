using ImageService.Controller;
using ImageService;
using ImageService.Controller.Handlers;
using ImageService.Server;
using ImageService.Infrastructure;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Windows.Input;

namespace ImageService.Server
{
    /// <summary>
    /// server class
    /// </summary>
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private string[] seperatedPaths;
        private byte[] toClient = { };
        private Settings settingsObj;
        #endregion

        #region Properties
        /// <summary>
        /// The event that notifies about a new Command being recieved
        /// </summary>
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logger">logging service</param>
        public ImageServer(ILoggingService logger)
        {
            string str = "";
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            this.m_logging = logger;
            //split all directories to handle from the appconfig
            string paths = ConfigurationManager.AppSettings["Handler"];
            this.seperatedPaths = paths.Split(';');
            //save the outputDir from appconfig
            string outPutDir = ConfigurationManager.AppSettings["outputDir"];
            //save the source name from appconfig
            string sourceName = ConfigurationManager.AppSettings["SourceName"];
            //save the log name from appconfig
            string logNmae = ConfigurationManager.AppSettings["LogName"];
            //extract thumbnail size from appconfig
            string thumbnailSize = ConfigurationManager.AppSettings["ThumbnailSize"];
            //create a new modal - send as inputs the outputDir path and desiered thumbnail size
            IImageServiceModal imageModal = new ImageServiceModal(outPutDir, Int32.Parse(thumbnailSize));
            this.m_controller = new ImageController(imageModal);
            //create a direcotry handler for each path in the handler section extracted from the appconfig
            foreach (string path in seperatedPaths)
            {
                CreateHandler(path);
                str = enc.GetString(toClient);
                toClient = Encoding.ASCII.GetBytes(str + path + ";");
            }
            this.settingsObj = new Settings();
            settingsObj.OutputDir = outPutDir;
            settingsObj.SourceName = sourceName;
            settingsObj.LogName = logNmae;
            settingsObj.ThumbSize = thumbnailSize;
            settingsObj.Handlers = enc.GetString(toClient);
            Thread thread = new Thread(new ThreadStart(CreateTcpServer));
            thread.Start();
        }

        /// <summary>
        /// by a given path -create a handler for it
        /// </summary>
        /// <param name="path">path of direcory to handle</param>
        public void CreateHandler(string path)
        {
            IDirectoryHandler h = new DirectoyHandler(this.m_controller, this.m_logging, path);
            //subscribe the relevant functions to events
            CommandRecieved += h.OnCommandRecieved;
            h.DirectoryClose += ClosingServer;
            //after creating the handler - start actually handling it
            h.StartHandleDirectory(path);
        }

        /// <summary>
        /// closing the server function
        /// </summary>
        /// <param name="sender">who invoked the event called this func</param>
        /// <param name="args">info</param>
        public void ClosingServer(object sender, DirectoryCloseEventArgs args)
        {
            //log the relevant message
            m_logging.Log(args.Message, Logging.Modal.MessageTypeEnum.INFO);
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            //unsubscribe the functions from relevant events
            CommandRecieved -= handler.OnCommandRecieved;
            handler.DirectoryClose -= ClosingServer;
        }

        /// <summary>
        /// destructor for the server
        /// </summary>
        ~ImageServer()
        {
            //for each directory - close it's direcotry handler
            foreach (string path in this.seperatedPaths)
            {
                CommandRecievedEventArgs closeCommandArgs = new
                    CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, path);
                this.CommandRecieved?.Invoke(this, closeCommandArgs);
            }
        }
        private void CreateTcpServer()
        {
            TcpTimeServer tcpServer = new TcpTimeServer();
            tcpServer.Start(this.m_logging);
            tcpServer.Send(Encoding.ASCII.GetBytes(this.settingsObj.ToJSON()), this.m_logging);
            //string command = tcpServer.GetCommand();
            //m_logging.Log(command, Logging.Modal.MessageTypeEnum.INFO);
        }
        internal class Settings
        {
            private string _OutputDir;
            public string OutputDir
            {
                get { return _OutputDir; }
                set { _OutputDir = value; }
            }
            private string _SourceName;
            public string SourceName
            {
                get { return _SourceName; }
                set { _SourceName = value; }
            }
            private string _LogName;
            public string LogName
            {
                get { return _LogName; }
                set { _LogName = value; }
            }
            private string _ThumbSize;
            public string ThumbSize
            {
                get { return _ThumbSize; }
                set { _ThumbSize = value; }
            }
            private string _Handlers;
            public string Handlers
            {
                get { return _Handlers; }
                set { _Handlers = value; }
            }
            public string ToJSON()
            {
                JObject settingObj = new JObject();
                settingObj["OutputDir"] = OutputDir;
                settingObj["SourceName"] = SourceName;
                settingObj["LogName"] = LogName;
                settingObj["ThumbSize"] = ThumbSize;
                settingObj["Handlers"] = Handlers;
                return settingObj.ToString();
            }
        }
    }
}

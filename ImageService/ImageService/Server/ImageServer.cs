using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Event;
using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using Communication;
using System.Threading.Tasks;
using ImageService.Controller.Handlers;
using ImageService.Controller;
using ImageService.Logging;
using ImageService.Modal;
using Newtonsoft.Json;

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
        private TcpTimeServer tcpServer;
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

            ClientHandler.DebugClientHandler += this.LogTheFuckingMessageDebug;
           // ClientHandler.GotCommandFromGui += this.GetCommandFromService;
            new Task(() =>
            {
                this.tcpServer = new TcpTimeServer();
                this.tcpServer.NewClientConnected += this.SendSettingsAndLog;
                this.tcpServer.PassInfoFromClientHandlerToServer += this.GetCommandFromService;
                this.tcpServer.Start();
            }).Start();
        }

        private void LogTheFuckingMessageDebug(object sender, string e)
        {
            this.m_logging.Log(e, Logging.Modal.MessageTypeEnum.INFO);
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
        /// letting the server know a handler was closed
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

        private void GetCommandFromService(object sender, string jsonCommand)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(jsonCommand);
           // string commandID = (string)json["CommandID"];
          //  string closeCommand = CommandEnum.CloseCommand.ToString();
           // if (commandID.Equals(closeCommand))
           if ((int)json["CommandID"] == (int)CommandEnum.CloseCommand)
            {
                CommandRecievedEventArgs args = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, (string)json["Handler"]);
                this.CommandRecieved?.Invoke(this, args);
                tcpServer.SendToAllClients(JsonConvert.SerializeObject(json));
            }
        }

        private void SendSettingsAndLog(object sender, IClientHandler client)
        {
            bool success;
            string setting = m_controller.ExecuteCommand((int)CommandEnum.GetConfigCommand, null, out success);
           // IClientHandler handler = (IClientHandler)sender;
            client.SendCommand(setting);
        }

        
    }
}

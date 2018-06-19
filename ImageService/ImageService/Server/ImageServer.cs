using Infrastructure.Enums;
using Infrastructure.Event;
using System;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Communication;
using System.Threading.Tasks;
using ImageService.Controller.Handlers;
using ImageService.Controller;
using ImageService.Logging;
using ImageService.Modal;
using Newtonsoft.Json;
using Infrastructure.Modal;

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
        private TcpAndroidServer androidServer;
        private string[] seperatedPaths;
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
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            this.m_logging = logger;
            this.m_logging.MessageRecieved += WriteLogMessage;
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
            IImageServiceModal imageModal = new ImageServiceModal(outPutDir, Int32.Parse(thumbnailSize), m_logging);
            this.m_controller = new ImageController(imageModal);
            //create a direcotry handler for each path in the handler section extracted from the appconfig
            foreach (string path in seperatedPaths)
            {
                CreateHandler(path);
            }
            //creates the tcpServer and register to it's events of getting a new client and reciving commands
            new Task(() =>
            {
                this.androidServer = new TcpAndroidServer(this.seperatedPaths);
                this.tcpServer = new TcpTimeServer();
                this.tcpServer.NewClientConnected += this.SendSettingsAndLog;
                this.tcpServer.PassInfoFromClientHandlerToServer += this.GetCommandFromService;
                this.androidServer.Start();
                this.tcpServer.Start();
            }).Start();
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
            bool result;
            //log the relevant message
            m_logging.Log(args.Message, MessageTypeEnum.INFO);
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            //calls the controller to remove the directory from
            //the config so it wont be showing at the next connecting client
            string[] forRemove = { args.DirectoryPath };
            string deleteFromConfig = this.m_controller.
                ExecuteCommand((int)CommandEnum.RemoveHandlerFromConfig, forRemove, out result);
            this.m_logging.Log(deleteFromConfig, MessageTypeEnum.INFO);
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
            this.tcpServer.Close();
        }

        /// <summary>
        /// Gets a command to execute and called the service proj to do it.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="jsonCommand">The json command.</param>
        private void GetCommandFromService(object sender, string jsonCommand)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(jsonCommand);
            if ((int)json["CommandID"] == (int)CommandEnum.CloseCommand)
            {
                CommandRecievedEventArgs args = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, (string)json["Handler"]);
                this.CommandRecieved?.Invoke(this, args);
                tcpServer.SendToAllClients(JsonConvert.SerializeObject(json));
            }
        }

        /// <summary>
        /// Sends the settings and log.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="client">The client.</param>
        private void SendSettingsAndLog(object sender, IClientHandler client)
        {
            bool success;
            //genertate string (serialize to json of all settings from the appconfig)
            string setting = m_controller.ExecuteCommand((int)CommandEnum.GetConfigCommand, null, out success);
            client.SendCommand(setting);
            string log = m_controller.ExecuteCommand((int)CommandEnum.LogCommand, null, out success);
            client.SendCommand(log);
        }

        /// <summary>
        /// event function to handel the sending of the update log list to all clients.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WriteLogMessage(Object sender, MessageRecievedEventArgs e)
        {
            bool success;
            string log = m_controller.ExecuteCommand((int)CommandEnum.LogCommand, null, out success);
            try
            {
                tcpServer.SendToAllClients(log);
            } catch (Exception exception)
            {
                return;
            }
        }

    }
}

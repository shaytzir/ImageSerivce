using Infrastructure.Enums;
using Infrastructure.Event;
using System;
using System.IO;
using ImageService.Logging;
using System.Collections.Generic;
using ImageService.Commands;
using ImageService.Logging.Modal;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// directory handler implemntation
    /// </summary>
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        // The Image Processing Controller
        private IImageController m_controller;
        private ILoggingService m_logging;
        // The Path of directory
        private string m_path;
        //valid extension for the handler to filter
        private string[] ext;
        //list of systemWatcher - each for different extension
        private List<FileSystemWatcher> m_dirWatcher;
        #endregion

        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="controller">Image Controller</param>
        /// <param name="logger">a Logging Service</param>
        /// <param name="DirPath">a path for this handler to watch</param>
        public DirectoyHandler(IImageController controller, ILoggingService logger, string DirPath)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            this.m_path = DirPath;
            this.ext = new string[] { "*.bmp", "*.gif", "*.png", "*.jpg" };
            this.m_dirWatcher = new List<FileSystemWatcher>();
        }

        /// <summary>
        /// starting to handle a specific directory
        /// </summary>
        /// <param name="dirPath">the path of the directory to handle</param>
        public void StartHandleDirectory(string dirPath)
        {
            //create list of file Watcher - each watcher filters another extensions
            foreach (string extension in ext)
            {
                FileSystemWatcher watcher = new FileSystemWatcher(m_path);
                //filter the watcher according to a specific extension
                watcher.Filter = extension;
                //add the Oncreated func to subscribe to the event of the watcher
                watcher.Created += new FileSystemEventHandler(OnCreated);
                // Begin watching.
                watcher.EnableRaisingEvents = true;
                //add the working watcher to the list
                m_dirWatcher.Add(watcher);
            }
            this.m_logging.Log("Watching over " + this.m_path, Logging.Modal.MessageTypeEnum.INFO);
        }

        /// <summary>
        /// on created func - being notified when a new valid file is added to the directory
        /// </summary>
        /// <param name="source">object who raised the event</param>
        /// <param name="e">info about the new file</param>
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            //got here because a new file was created - get the right int number from the command enum
            int created = (int)CommandEnum.NewFileCommand;
            //send the path of the new created file at args[0]
            string[] args = { e.FullPath };
            //execute the relevant command and get logMessage to print and result as feedback of success/failure
            CommandRecievedEventArgs newFileCreated = new CommandRecievedEventArgs(created, args, this.m_path);
            this.OnCommandRecieved(this, newFileCreated);
        }


        /// <summary>
        /// The Event that will be activated upon new Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            int command = (int)e.CommandID;
            string dirPath = e.RequestDirPath;
            //incase the path matches the directory path - execute
            if (dirPath.Equals(this.m_path))
            {
                //if the command is to close - close
                if (command == (int)CommandEnum.CloseCommand)
                {
                    HandleClosing();
                }
                //otherwise - handle as default -adding
                else
                {
                    HandleAdding(e);
                }
            }
        }


        /// <summary>
        /// handling the situation where a new file was added
        /// </summary>
        /// <param name="e">info for execution</param>
        public void HandleAdding(CommandRecievedEventArgs e)
        {

            //creating a task for this specific addition
            Task newTask = new Task(() =>
            {
                bool result;
                string logMessage = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                if (result)
                {
                    this.m_logging.Log(logMessage, Logging.Modal.MessageTypeEnum.INFO);
                }
                else
                {
                    this.m_logging.Log(logMessage, Logging.Modal.MessageTypeEnum.FAIL);
                }
            });
            //running the task
            newTask.Start();
        }


        /// <summary>
        /// handling the situation of closing the directory
        /// </summary>
        public void HandleClosing()
        {
            string message;
            try
            {
                //make each watcher stop filtering and watching
                foreach (FileSystemWatcher watcher in this.m_dirWatcher)
                {
                    watcher.EnableRaisingEvents = false;
                }
                //log that this directory is closed
                message = m_path + "is no longer watched";
                DirectoryCloseEventArgs closing = new DirectoryCloseEventArgs(m_path, message);
                DirectoryClose?.Invoke(this, closing);
            }
            catch (Exception e)
            {
                //couldnt close the directory - log it
                message = "Error while trying to close " + m_path;
                m_logging.Log(message, MessageTypeEnum.INFO);
            }
            finally
            {
                //unsubscribe the "onCreated" func from the event
                foreach (FileSystemWatcher watch in this.m_dirWatcher)
                {
                    watch.Created -= new FileSystemEventHandler(OnCreated);
                }
            }
        }
    }
}
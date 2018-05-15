using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using ImageService.Logging;
using ImageService.Server;
using ImageService.Logging.Modal;

namespace ImageService
{
    /// <summary>
    /// class of the image service
    /// </summary>
    public partial class ImageService : ServiceBase
    {
        private int eventId = 1;
        private ILoggingService loggerService;


        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        /// <summary>
        /// constuctor
        /// </summary>
        /// <param name="args">args from commandline</param>
        public ImageService(string[] args)
        {
            InitializeComponent();
            //incase there is no appconfig
            string eventSourceName = "MySource";
            string logName = "MyNewLog";
            eventSourceName = ConfigurationManager.AppSettings["SourceName"];
            logName = ConfigurationManager.AppSettings["LogName"];
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }

        /// <summary>
        /// on start function - starts when the modal is on
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            this.loggerService = new LoggingService();
            this.loggerService.MessageRecieved += WriteLogMessage;
            //create a server for this service
            ImageServer server = new ImageServer(loggerService);
        }


        /// <summary>
        /// on stop - when service is off
        /// </summary>
        protected override void OnStop()
        {
            eventLog1.WriteEntry("In onStop.");
            loggerService.MessageRecieved -= WriteLogMessage;
        }

        /// <summary>
        /// logs messages
        /// </summary>
        /// <param name="sender">invoking object of the event</param>
        /// <param name="e">info of the message to log</param>
        public void WriteLogMessage(Object sender, MessageRecievedEventArgs e)
        {
            eventLog1.WriteEntry(e.Message);
        }

        internal class Settings
        {
        }
    }
}

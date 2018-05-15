using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    public class SettingModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        private TcpTimeClient tcpClient = TcpTimeClient.Instance;
        public SettingModel()
        {
            Settings settingsObject = tcpClient.SettingObj;
            m_LogName = settingsObject.LogName;
            m_Handlers = settingsObject.Handlers.Split(';');
            m_SourceName = settingsObject.SourceName;
            m_OutputDir = settingsObject.OutputDir;
            m_ThumbnailSize = settingsObject.ThumbSize;
        }
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        private string m_OutputDir;
        public string OutputDir
        {
            get
            {
                return m_OutputDir;
            }
            set
            {
                m_OutputDir = value;
            }
        }
        private string m_LogName;
        public string LogName
        {
            get
            {
                return m_LogName;
            }
            set
            {
                m_LogName = value;
            }
        }
        private string m_SourceName;
        public string SourceName
        {
            get
            {
                return m_SourceName;
            }
            set
            {
                m_SourceName = value;
            }
        }
        private string m_ThumbnailSize;
        public string ThumbnailSize
        {
            get
            {
                return m_ThumbnailSize;
            }
            set
            {
                m_ThumbnailSize = value;
            }
        }
        private string[] m_Handlers;
        public string[] Handlers
        {
            get
            {
                return m_Handlers;
            }
            set
            {
                this.NotifyPropertyChanged("Handlers");
            }
        }
        private string handlerToRemove;
        public string _HandlersToRemove
        {
            get
            {
                return this.handlerToRemove;
            }
            set
            {
                this.handlerToRemove = value;
                OnPropertyChanged("_HandlersToRemove");
            }
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public void RemoveHendler(string handler)
        {
            tcpClient.SendCommand("Remove " + handler);
        }
    }
}

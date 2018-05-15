using System.Collections.ObjectModel;
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
            m_Handlers = new ObservableCollection<string>();
            string[] hendlers = settingsObject.Handlers.Split(';');
            for (int i = 0; i < hendlers.Length; i++)
            {
                m_Handlers.Add(hendlers[i]);
            }
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
                this.NotifyPropertyChanged("OutputDir");
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
                this.NotifyPropertyChanged("LogName");
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
                this.NotifyPropertyChanged("SourceName");
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
                this.NotifyPropertyChanged("ThumbnailSize");
            }
        }
        private ObservableCollection<string> m_Handlers;
        public ObservableCollection<string> Handlers
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

using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class MainWindowModel : IMainWindowModel
    {

        private bool m_Connect;

        public MainWindowModel()
        {
            GuiClient client = GuiClient.Instance;
            m_Connect = client.Connected;

        }
        public bool Connected
        {
            get { return m_Connect; }
            set
            {
                m_Connect = value;
                NotifyPropertyChanged("Connected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}


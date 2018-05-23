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

        /// <summary>
        /// Counstructor.
        /// </summary>
        public MainWindowModel()
        {
            GuiClient client = GuiClient.Instance;
            m_Connect = client.Connected;

        }
        /// <summary>
        /// Gets or sets a value indicating whether this MainWindowModel is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected
        {
            get { return m_Connect; }
            set
            {
                m_Connect = value;
                NotifyPropertyChanged("Connected");
            }
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}


using ImageService.Logging.Modal;
using ImageServiceGUI.Model;
using Infrastructure.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    class LogViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        private LogModel _LogModel;
        /// <summary>
        /// Constructor
        /// </summary>
        public LogViewModel()
        {
            this._LogModel = new LogModel();
            this._LogModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        /// <summary>
        /// Gets the vm output dir.
        /// </summary>
        /// <value>
        /// The vm output dir.
        /// </value>
        public ObservableCollection<MessageRecievedEventArgs> VM_Logs
        {
            get { return this._LogModel.Logs; }
        }
    }
}

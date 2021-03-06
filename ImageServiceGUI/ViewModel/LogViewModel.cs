﻿using Infrastructure.Enums;
using Infrastructure.Modal;
using Infrastructure.Event;
using ImageServiceGUI.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    class LogViewModel : ILogViewModel
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private ILogModel _LogModel;

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
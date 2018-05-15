using ImageServiceGUI.Model;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModels
{
    public class SettingViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        private SettingModel _SettingModel;
        public ICommand SubmitCommand { get; private set; }

        public SettingViewModel()
        {
            this.SubmitCommand = new DelegateCommand<object>(this.OnDelete, this.CanDelete);
            this._SettingModel = new SettingModel();
            SettingModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            this.VM_Handlers = this._SettingModel.Handlers;
        }

        public SettingModel SettingModel
        {
            get { return this._SettingModel; }
            set
            {
                this._SettingModel = value;
            }
        }

        public string VM_OutputDir;
        public string OutputDir
        {
            get { return this._SettingModel.OutputDir; }
            set
            {
                this.VM_OutputDir = value;
            }
        }
        public string VM_SourceName;
        public string SourceName
        {
            get { return this._SettingModel.SourceName; }
            set
            {
                this.VM_SourceName = value;
            }
        }
        public string VM_LogName;
        public string LogName
        {
            get { return this._SettingModel.LogName; }
            set
            {
                this.VM_LogName = value;
            }
        }
        public string VM_ThumbnailSize;
        public string ThumbnailSize
        {
            get
            {
                return this._SettingModel.ThumbnailSize; }
            set
            {
                this.VM_ThumbnailSize = value;
            }
        }
        public string[] VM_Handlers { get; set; }
        private string handlerToRemove;
        public string VM_HandlersToRemove {
            get
            {
                return this.handlerToRemove;
            }
            set
            {
                this.handlerToRemove = value;
                this._SettingModel.RemoveHendler(value);
            }
        }
        private void OnDelete(object obj)
        {
        }
        private bool CanDelete(object obj)
        {
            if (string.IsNullOrEmpty(this.handlerToRemove))
            {
                return false;
            }
            return true;
        }

    }
}

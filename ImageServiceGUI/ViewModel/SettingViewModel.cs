using ImageServiceGUI.Model;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            var command = this.SubmitCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
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
        }

        public SettingModel SettingModel
        {
            get { return this._SettingModel; }
        }

        public string VM_OutputDir
        {
            get { return this._SettingModel.OutputDir; }
        }
        public string VM_SourceName
        {
            get { return this._SettingModel.SourceName; }
        }
        public string VM_LogName
        {
            get { return this._SettingModel.LogName; }
        }
        public string VM_ThumbnailSize
        {
            get
            {
                return this._SettingModel.ThumbnailSize;
            }
        }
        public ObservableCollection<string> VM_Handlers {
            get
            {
                return this._SettingModel.Handlers;
            }
        }
        private string handlerToRemove;
        public string VM_HandlersToRemove {
            get
            {
                return this.handlerToRemove;
            }
            set
            {
                this.handlerToRemove = value;
                this.NotifyPropertyChanged("handlerToRemove");
            }
        }
        private void OnDelete(object obj)
        {
            this._SettingModel.RemoveHendler(this.handlerToRemove);
        }
        private bool CanDelete(object obj)
        {
            if (this.handlerToRemove != null)
            {
                return true;
            }
            return false;
        }

    }
}

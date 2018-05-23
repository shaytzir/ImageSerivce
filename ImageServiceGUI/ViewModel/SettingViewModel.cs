using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Infrastructure.Enums;
using Infrastructure.Event;
using ImageServiceGUI.Communication;
using Newtonsoft.Json;
using Infrastructure;
using Newtonsoft.Json.Linq;

namespace ImageServiceGUI.ViewModels
{
    public class SettingViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        private SettingModel _SettingModel;

        public SettingViewModel()
        {
            this._SettingModel = new SettingModel();
            this._SettingModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            this.SubmitCommand = new DelegateCommand<object>(this.OnDelete, this.CanDelete);
        }

        public ICommand SubmitCommand { get; private set; }


        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            var command = this.SubmitCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
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

        private string selectedItem;
        public string SelectedItem
        {
            set
            {
                this.selectedItem = value;
                this.NotifyPropertyChanged("SelectedItem");
            }
            get { return this.selectedItem; }
        }
        private void OnDelete(object obj)
        {
            string[] handlerName = { this.selectedItem };
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, handlerName,this.selectedItem);
            JObject json= new JObject();
            json["CommandID"] = (int)CommandEnum.CloseCommand;
            json["Handler"] = selectedItem;
            string converted = JsonConvert.SerializeObject(json);
            GuiClient.Instance.Comm.SendCommand(converted);
        }


        private bool CanDelete(object obj)
        {
            if (this.selectedItem != null)
            {
                return true;
            }
            return false;
        }

    }
}

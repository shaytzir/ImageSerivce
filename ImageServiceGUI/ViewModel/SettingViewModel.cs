using ImageServiceGUI.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Infrastructure.Enums;
using Infrastructure.Event;
using ImageServiceGUI.Communication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ImageServiceGUI.ViewModel;

namespace ImageServiceGUI.ViewModels
{
    public class SettingViewModel : ISettingViewModel
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        private ISettingModel _SettingModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingViewModel()
        {
            this._SettingModel = new SettingModel();
            this._SettingModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            this.RemoveHandlerCommand = new DelegateCommand<object>(this.OnDelete, this.CanDelete);
            
        }

        /// <summary>
        /// Gets the submit command.
        /// </summary>
        /// <value>
        /// The submit command.
        /// </value>
        public ICommand RemoveHandlerCommand { get; private set; }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            var command = this.RemoveHandlerCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Gets the vm output dir.
        /// </summary>
        /// <value>
        /// The vm output dir.
        /// </value>
        public string VM_OutputDir
        {
            get { return this._SettingModel.OutputDir; }
        }

        /// <summary>
        /// Gets the name of the vm source.
        /// </summary>
        /// <value>
        /// The name of the vm source.
        /// </value>
        public string VM_SourceName
        {
            get { return this._SettingModel.SourceName; }
        }

        /// <summary>
        /// Gets the name of the vm log.
        /// </summary>
        /// <value>
        /// The name of the vm log.
        /// </value>
        public string VM_LogName
        {
            get { return this._SettingModel.LogName; }
        }

        /// <summary>
        /// Gets the size of the vm thumbnail.
        /// </summary>
        /// <value>
        /// The size of the vm thumbnail.
        /// </value>
        public string VM_ThumbnailSize
        {
            get
            {
                return this._SettingModel.ThumbnailSize;
            }
        }

        /// <summary>
        /// Gets the vm handlers.
        /// </summary>
        /// <value>
        /// The vm handlers.
        /// </value>
        public ObservableCollection<string> VM_Handlers {
            get
            {
                return this._SettingModel.Handlers;
            }
        }

        private string selectedItem;

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public string SelectedItem
        {
            set
            {
                this.selectedItem = value;
                this.NotifyPropertyChanged("SelectedItem");
            }
            get { return this.selectedItem; }
        }

        /// <summary>
        /// when deleting a handler.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnDelete(object obj)
        {
            string[] handlerName = { this.selectedItem };
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, handlerName,this.selectedItem);
            JObject json= new JObject();
            json["CommandID"] = (int)CommandEnum.CloseCommand;
            json["Handler"] = selectedItem;
            string converted = JsonConvert.SerializeObject(json);
            //sends command to the server to delete this handler from the handlers to watch
            GuiClient.Instance.Comm.SendCommand(converted);
        }

        /// <summary>
        /// Determines whether this button can delete the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified object; otherwise, <c>false</c>.
        /// </returns>
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
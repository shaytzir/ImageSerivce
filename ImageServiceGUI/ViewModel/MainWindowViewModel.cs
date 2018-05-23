using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using ImageServiceGUI.Model;
using System;

namespace ImageServiceGUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private MainWindowModel model;
        public MainWindowViewModel()
        {
            this.model = new MainWindowModel();
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
            //this.SettingViewModel = new SettingViewModel();
            //this.SettingViewModel.PropertyChanged += PropertyChanged;
        }
        public string VM_Connected
        {
            get
            {
                if (this.model.Connected == true)
                {
                    return "RosyBrown";
                } else
                {
                    return "Gray";
                }
            }
        }
        public ICommand RemoveCommand { get; private set; }

        public SettingViewModel SettingViewModel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

       /* private string BuildResultString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.SettingViewModel.SettingModel.Handlers);
            return builder.ToString();
        }*/
    }
}

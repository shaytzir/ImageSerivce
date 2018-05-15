using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            this.SettingViewModel = new SettingViewModel();
            this.SettingViewModel.PropertyChanged += PropertyChanged;
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        public ICommand RemoveCommand { get; private set; }

        public SettingViewModel SettingViewModel { get; set; }

        private void OnReset()
        {
            this.SettingViewModel.SettingModel = new SettingModel();
        }

        private string BuildResultString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.SettingViewModel.SettingModel.Handlers);
            return builder.ToString();
        }
    }
}

using ImageServiceGUI.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        ObservableCollection<string> HandlerList = new ObservableCollection<string>();
        SettingViewModel settingViewModel;
        public SettingsView()
        {
            InitializeComponent();
            this.settingViewModel = new SettingViewModel();
            this.DataContext = this.settingViewModel;
            string[] handlersList = settingViewModel.VM_Handlers;
            for(int i = 0; i < handlersList.Length; i++)
            {
                if (handlersList[i] != "")
                {
                    this.HandlerList.Add(handlersList[i]);
                }
            }
            LBHandlers.ItemsSource = this.HandlerList;
        }
        private void btnRemoveHandler_Click(object sender, RoutedEventArgs e)
        {
            if (LBHandlers.SelectedItem != null)
            {
                this.settingViewModel.VM_HandlersToRemove = LBHandlers.SelectedItem as string;
                HandlerList.Remove(LBHandlers.SelectedItem as string);
            }
        }

        private void Handlers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBHandlers.SelectedItem != null)
            {
                this.settingViewModel.VM_HandlersToRemove = LBHandlers.SelectedItem as string;
            }
        }
    }
}

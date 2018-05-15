using ImageServiceGUI.ViewModels;

namespace ImageServiceGUI
{
    public class SettingViewDesignViewModel
    {
        public SettingViewDesignViewModel()
        {
            this.SettingViewModel = new SettingViewModel();
        }

        public SettingViewModel SettingViewModel { get; set; }
    }
}
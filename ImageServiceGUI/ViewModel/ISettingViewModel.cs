using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    interface ISettingViewModel : INotifyPropertyChanged
    {
        string VM_OutputDir { get; }
        string VM_SourceName { get; }
        string VM_LogName { get; }
        string VM_ThumbnailSize { get; }
        ObservableCollection<string> VM_Handlers { get; }
    }
}

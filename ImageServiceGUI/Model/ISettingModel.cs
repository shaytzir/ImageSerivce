using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    interface ISettingModel : INotifyPropertyChanged
    {

        string SourceName { set; get; }
        string LogName { set; get; }
        string OutputDir { set; get; }
        string ThumbnailSize { set; get; }
        string _HandlersToRemove { set; get; }
        ObservableCollection<string> Handlers { get; set; }
    }
}

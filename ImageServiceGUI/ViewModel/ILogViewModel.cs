using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    interface ILogViewModel : INotifyPropertyChanged
    {
        ObservableCollection<MessageRecievedEventArgs> VM_Logs { get;}
    }
}

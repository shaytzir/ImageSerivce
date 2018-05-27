using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    interface IMainWindowViewModel : INotifyPropertyChanged
    {
        string VM_Connected { get; }
    }
}

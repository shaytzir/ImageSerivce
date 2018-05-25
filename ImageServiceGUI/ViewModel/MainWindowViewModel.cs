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
        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel()
        { 
            this.model = new MainWindowModel();
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }
        /// <summary>
        /// sets the color of the background depends on the connection
        /// </summary>
        /// <value>
        /// the color of the background
        /// </value>
        public string VM_Connected
        {
            get
            {
                if (this.model.Connected == true)
                {
                    return "DarkOliveGreen";
                } else
                {
                    return "Gray";
                }
            }
        }

        /// <summary>
        /// Gets the remove command.
        /// </summary>
        /// <value>
        /// The remove command.
        /// </value>
        public ICommand RemoveCommand { get; private set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}

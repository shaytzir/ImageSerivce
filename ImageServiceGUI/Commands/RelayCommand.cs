using ImageService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        public RelayCommand(Action<object> execute)
        {
            this.execute = execute;
        }
        public void Execute(string[] args)
        {
            this.execute(args);
        }
    }
}

using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        private IImageServiceModal m_modal;
        public GetConfigCommand(IImageServiceModal modal)
        {
            this.m_modal = modal;
        }

        public string Execute(string[] args, out bool result)
        {
            return m_modal.GetConfig(out result);
        }
    }
}

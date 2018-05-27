using ImageService.Modal;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        private IImageServiceModal m_modal;
        public LogCommand(IImageServiceModal modal)
        {
            this.m_modal = modal;
        }

        /// <summary>
        /// Return the log from the modal.
        /// </summary>
        public string Execute(string[] args, out bool result)
        {
            return m_modal.GetLog(out result);
        }
    }
}

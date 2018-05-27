using ImageService.Modal;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        private IImageServiceModal m_modal;
        public GetConfigCommand(IImageServiceModal modal)
        {
            this.m_modal = modal;
        }

        /// <summary>
        /// Return the configuration from the modal.
        /// </summary>
        public string Execute(string[] args, out bool result)
        {
            return m_modal.GetConfig(out result);
        }
    }
}

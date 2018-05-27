using System;

namespace Communication
{
    /// <summary>
    /// client handler interface
    /// </summary>
    public interface IClientHandler
    {
        /// <summary>
        /// Occurs when the handler got command from GUI.
        /// </summary>
        event EventHandler<string> GotCommandFromGui;
        /// <summary>
        /// Occurs when the handler is closing.
        /// </summary>
        event EventHandler HandlerClosed;
        /// <summary>
        /// Handles the client - trying to read messages from client.
        /// </summary>
        void HandleClient();
        /// <summary>
        /// Sends the command to the client.
        /// </summary>
        /// <param name="commandToTcpClient">The command to TCP client.</param>
        void SendCommand(string commandToTcpClient);
        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();
    }
}
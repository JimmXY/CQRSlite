using System.Threading;
using System.Threading.Tasks;

namespace CQRSlite.Commands
{
    /// <summary>
    /// Defines a command sender.
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Send a command to a single command handler function.
        /// </summary>
        /// <typeparam name="TCommandType">Command type</typeparam>
        /// <param name="command">Command object to be sent</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Task representing sending</returns>
        Task Send<TCommandType>(TCommandType command, CancellationToken cancellationToken = default) where TCommandType : class, ICommand;
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace CQRSlite.Events
{
    /// <summary>
    /// Defines an event publisher
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publish an event to zero to many handler functions.
        /// </summary>
        /// <typeparam name="TEventType">Event type</typeparam>
        /// <param name="event">Event object to be sent</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Task representing publishing</returns>
        Task Publish<TEventType>(TEventType @event, CancellationToken cancellationToken = default) where TEventType : class, IEvent;
    }
}
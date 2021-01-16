using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CQRSlite.Commands;
using CQRSlite.Events;
using CQRSlite.Messages;
using CQRSlite.Queries;

using Microsoft.Extensions.DependencyInjection;

namespace CQRSlite.Routing
{
    /// <summary>
    /// Service provider based router implementation for sending commands and publishing events.
    /// </summary>
    public class ServiceProviderRouter : ICommandSender, IEventPublisher, IQueryProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ServiceProviderRouter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task<TResponse> Query<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetService<ICancellableQueryHandler<IQuery<TResponse>, TResponse>>() ?? throw new InvalidOperationException($"No handler registered for {(typeof(IQuery<TResponse>)).FullName}");
            return handler.Handle(query, cancellationToken);
        }

        Task IEventPublisher.Publish<TEventType>(TEventType @event, CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var listeners = scope.ServiceProvider.GetServices<ICancellableEventHandler<TEventType>>();
            return Task.WhenAll(listeners.Select(s => s.Handle(@event, cancellationToken)));
        }

        Task ICommandSender.Send<TCommandType>(TCommandType command, CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetService<ICancellableCommandHandler<TCommandType>>() ?? throw new InvalidOperationException($"No handler registered for {(typeof(TCommandType)).FullName}");
            return handler.Handle(command, cancellationToken);
        }
    }
}

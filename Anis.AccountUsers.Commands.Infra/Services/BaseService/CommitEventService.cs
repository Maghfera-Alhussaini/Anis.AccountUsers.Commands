using Anis.AccountUsers.Commands.Application.Contracts.Repositories;
using Anis.AccountUsers.Commands.Application.Services.BaseServices;
using Anis.AccountUsers.Commands.Application.Services.ServiceBus;
using Anis.AccountUsers.Commands.Domain.Entities;
using Anis.AccountUsers.Commands.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Infra.Services.BaseService
{
    public class CommitEventService : ICommitEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        public CommitEventService(IUnitOfWork unitOfWork, IServiceBusPublisher serviceBusPublisher)
        {
            _unitOfWork = unitOfWork;
            _serviceBusPublisher = serviceBusPublisher;
        }

        public async Task CommitNewEventsAsync(Account aggregate)
        {
            var newEvents = aggregate.GetUncommittedEvents();

            await _unitOfWork.Events.AddRangeAsync(newEvents);

            var messages = OutboxMessage.ToManyMessages(newEvents);

            await _unitOfWork.OutboxMessages.AddRangeAsync(messages);

            await _unitOfWork.SaveChangesAsync();

            aggregate.MarkChangesAsCommitted();

            _serviceBusPublisher.StartPublish();
        }
    }
}

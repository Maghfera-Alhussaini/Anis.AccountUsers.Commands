using Anis.AccountUsers.Commands.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Application.Contracts.Services.ServiceBus
{
   public interface IServiceBusEventSender
    {
        Task SendEventAsync(Event @event);
    }
}

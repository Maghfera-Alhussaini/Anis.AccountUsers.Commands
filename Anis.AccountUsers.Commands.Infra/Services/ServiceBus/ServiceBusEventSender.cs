using Anis.AccountUsers.Commands.Application.Contracts.Services.ServiceBus;
using Anis.AccountUsers.Commands.Domain.Events;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Infra.Services.ServiceBus
{
    public class ServiceBusEventSender : IServiceBusEventSender
    {
        private readonly ServiceBusSender _sender;
        public ServiceBusEventSender(IConfiguration configuration, ServiceBusClient serviceBusClient)
        {
            var topic = configuration["ServiceBus:Topic"];
            _sender = serviceBusClient.CreateSender(configuration["ServiceBus:Topic"]);
        }
        public async Task SendEventAsync(Event @event)
        {
            var body = new MessageBody()
            {
                AggregateId = @event.AggregateId,
                DateTime = @event.DateTime,
                Sequence = @event.Sequence,
                Type = @event.Type.ToString(),
                UserId = @event.UserId,
                Version = @event.Version,
                Data = ((dynamic)@event).Data
            };
            var messageBody = JsonConvert.SerializeObject(body, new StringEnumConverter());
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody))
            {
                CorrelationId = @event.AggregateId.ToString(),
                MessageId = @event.AggregateId.ToString(),
                PartitionKey = @event.AggregateId.ToString(),
                SessionId = @event.AggregateId.ToString(),
                Subject = @event.Type.ToString(),
                ApplicationProperties =
                {
                    { nameof(@event.AggregateId), @event.AggregateId },
                    {nameof(@event.Sequence), @event.Sequence },
                    {nameof(@event.Version), @event.Version }

                }
            };
            await _sender.SendMessageAsync(message);
        }
    }
}

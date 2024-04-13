using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Anis.AccountUsers.Commands.Infra.Services.ServiceBus;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Test.Live.Asserts
{
    public static class MessageAssert
    {
        public static void AssertEquality(
            UserDeletedFromAccount? userDeletedFromAccount,
            ServiceBusReceivedMessage? serviceBusReceivedMessage
            )
        {
            Assert.NotNull(serviceBusReceivedMessage);
            Assert.NotNull(userDeletedFromAccount);       
            var body = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(serviceBusReceivedMessage?.Body));
            Assert.NotNull(body);
            BaseAssert(userDeletedFromAccount, serviceBusReceivedMessage, body);
            var eventData = userDeletedFromAccount.Data;

            var messageData = JsonConvert.DeserializeObject<UserDeletedFromAccountData>(value: body.Data!.ToString()!);

            Assert.NotNull(messageData);

            Assert.Equal(eventData.UserId, messageData.UserId);
            Assert.Equal(eventData.Type, messageData.Type);

        }

        public static void AssertEquality(
           UserAssignedToAccount? userAssignedToAccount,
           ServiceBusReceivedMessage? serviceBusReceivedMessage
           )
        {
            Assert.NotNull(serviceBusReceivedMessage);
            Assert.NotNull(userAssignedToAccount);
            var body = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(serviceBusReceivedMessage?.Body));
            Assert.NotNull(body);
            BaseAssert(userAssignedToAccount, serviceBusReceivedMessage, body);
            var eventData = userAssignedToAccount.Data;

            var messageData = JsonConvert.DeserializeObject<UserAssignedToAccountData > (value: body.Data!.ToString()!);

            Assert.NotNull(messageData);

            Assert.Equal(eventData.UserId, messageData.UserId);
            Assert.Equal(eventData.Type, messageData.Type);
        }
        private static void BaseAssert(Event? @event, ServiceBusReceivedMessage? message, MessageBody? body)
        {
            Assert.NotNull(@event);
            Assert.NotNull(message);
            Assert.Equal(@event.Id.ToString(), message.CorrelationId);

            Assert.NotNull(body);
            Assert.NotNull(body.Data);

            Assert.Equal(@event.Sequence, body.Sequence);
            Assert.Equal(@event.Version, body.Version);
            Assert.Equal(@event.Type.ToString(), body.Type);
            Assert.Equal(@event.DateTime, body.DateTime, TimeSpan.FromMinutes(1));
        }
    }
}

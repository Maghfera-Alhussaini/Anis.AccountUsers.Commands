using Anis.AccountUsers.Commands.Domain.Entities;
using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Anis.AccountUsers.Commands.Domain.Events;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anis.AccountUsers.Commands.Test.Protos;

namespace Anis.AccountUsers.Commands.Test.Asserts
{
    public static class EventsAssert
    {
        public static void AssertEquality(
            DeleteUserFromAccountRequest request,
            Event? @event)
        {
            Assert.NotNull(request);
            Assert.NotNull(@event);

            var eventData = (UserDeletedFromAccount)@event;

            Assert.Equal(request.AccountId, eventData.AggregateId.ToString());
            Assert.Equal(request.UserId, eventData.Data.UserId.ToString());

        }
        public static void AssertEquality(
          AssignUserToAccountRequest request,   
          Event? @event)
        {
            Assert.NotNull(request);
            Assert.NotNull(@event);

            var eventData = (UserAssignedToAccount)@event;

            Assert.Equal(request.AccountId, eventData.AggregateId.ToString());
            Assert.Equal(request.UserId, eventData.Data.UserId.ToString());

        }
        public static void AssertEquality<T, TData>(
        Event? @event,
           OutboxMessage? message
        ) where T : Event<TData>
          where TData : IEventData
        {
            Assert.NotNull(@event);
            Assert.NotNull(message);
            Assert.NotNull(message.Event);

            Assert.Equal(@event.Sequence, message.Event.Sequence);
            Assert.Equal(1, message.Event.Version);
            Assert.Equal(@event.Type, message.Event.Type);
            Assert.Equal(@event.DateTime, message.Event.DateTime, precision: TimeSpan.FromMinutes(1));

            Assert.Equal(((T)@event).Data, ((T)message.Event).Data);
            Assert.Equal(@event.Id, message.Event.Id);
        }
    }
}

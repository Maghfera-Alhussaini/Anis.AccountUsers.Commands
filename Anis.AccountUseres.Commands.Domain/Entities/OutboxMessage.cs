using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Domain.Entities
{
    public class OutboxMessage
    {
        public static IEnumerable<OutboxMessage> ToManyMessages(IEnumerable<Event> events)
    => events.Select(e => new OutboxMessage(e));
        private OutboxMessage() { }
        public OutboxMessage(Event @event)
        {
            Event = @event;
        }
        public long Id { get; private set; }
        public Event? Event { get; private set; }
    }
}

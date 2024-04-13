using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Grpc.Protos.History;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Anis.AccountUsers.Commands.Grpc.Extensions
{
    public static class ModelExtensions
    {

        public static RepeatedField<EventMessage> ToOutputEvent(this RepeatedField<EventMessage> eventsOutput, IEnumerable<Event> events)
        {
            eventsOutput.AddRange(events.Select(e => new EventMessage
            {
                CorrelationId = e.Id.ToString(),
                DateTime = Timestamp.FromDateTime(DateTime.SpecifyKind(e.DateTime, DateTimeKind.Utc)),
                Data = JsonConvert.SerializeObject(((dynamic)e).Data, new StringEnumConverter()),
                Type = e.Type.ToString(),
                Sequence = e.Sequence,
                Version = e.Version,
                AggregateId = e.AggregateId.ToString(),
                UserId = e.UserId
            }));

            return eventsOutput;
        }

    }
}

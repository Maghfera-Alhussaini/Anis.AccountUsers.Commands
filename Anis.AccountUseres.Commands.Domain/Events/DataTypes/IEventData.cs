using Anis.AccountUsers.Commands.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Domain.Events.DataTypes
{
    public interface IEventData
    {
        [JsonIgnore]
        EventType Type { get; }
    }
}

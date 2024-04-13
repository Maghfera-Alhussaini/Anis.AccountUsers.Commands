using Anis.AccountUsers.Commands.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Domain.Events.DataTypes
{
    public record UserDeletedFromAccountData(Guid UserId): IEventData
    {
        public EventType Type => EventType.UserDeletedFromAccount;
    }
}

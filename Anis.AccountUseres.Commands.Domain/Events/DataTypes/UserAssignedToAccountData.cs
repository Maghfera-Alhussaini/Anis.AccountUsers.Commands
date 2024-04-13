using Anis.AccountUsers.Commands.Domain.Enums;

namespace Anis.AccountUsers.Commands.Domain.Events.DataTypes
{
    public record UserAssignedToAccountData(Guid UserId) : IEventData
    {
        public EventType Type => EventType.UserAssignedToAccount;
    }
}

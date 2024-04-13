using Anis.AccountUsers.Commands.Domain.Events.DataTypes;

namespace Anis.AccountUsers.Commands.Domain.Events
{
    public class UserAssignedToAccount : Event<UserAssignedToAccountData>
    {
        public UserAssignedToAccount(
         Guid aggregateId,
         string userId,
         UserAssignedToAccountData data,
         int sequence = 1,
         int version = 1
         ) : base(aggregateId, sequence, userId, data, version)
        {
        }

    }
}

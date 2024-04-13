using Anis.AccountUsers.Commands.Domain.Events.DataTypes;

namespace Anis.AccountUsers.Commands.Domain.Events
{
    public class UserDeletedFromAccount : Event<UserDeletedFromAccountData>
    {
        public UserDeletedFromAccount(
         Guid aggregateId,
         string userId,
         UserDeletedFromAccountData data,
         int sequence = 1,
         int version = 1
         ) : base(aggregateId, sequence, userId, data, version)
        {
        }

    }
}

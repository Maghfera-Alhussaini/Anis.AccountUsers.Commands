using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Test.Fakers
{
    public class UserAssignedToAccountFaker : CustomConstructorFaker<UserAssignedToAccount>
    {
        public UserAssignedToAccountFaker(Guid aggregateId, Guid userid, int sequence)
        {
            RuleFor(r => r.AggregateId, f => aggregateId);
            RuleFor(r => r.Sequence, sequence);
            RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
            RuleFor(r => r.Version, 1);
            RuleFor(r => r.DateTime, DateTime.UtcNow);
            RuleFor(r => r.Data, new UserAssignedToAccountDataFaker(userid).Generate());
        }
        public UserAssignedToAccountFaker()
        {
            RuleFor(r => r.AggregateId, f => f.Random.Guid());
            RuleFor(r => r.Sequence, 1);
            RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
            RuleFor(r => r.Version, 1);
            RuleFor(r => r.DateTime, DateTime.UtcNow);
            RuleFor(r => r.Data, new UserAssignedToAccountDataFaker().Generate());
        }

    }
    public class UserAssignedToAccountDataFaker : CustomConstructorFaker<UserAssignedToAccountData>
    {
        public UserAssignedToAccountDataFaker(Guid userId)
        {
            RuleFor(r => r.UserId, value: userId);
        }
        public UserAssignedToAccountDataFaker()
        {
            RuleFor(r => r.UserId, f => f.Random.Guid());
        }
    }
}

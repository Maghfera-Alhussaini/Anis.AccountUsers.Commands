using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Domain.Commands
{
    public interface IDeleteUserFromAccountCommand
    {
        Guid AccountId { get; }
        Guid UserId { get; }
    }
}

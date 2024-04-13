using Anis.AccountUsers.Commands.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Application.Services.BaseServices
{
    public interface ICommitEventService
    {
        Task CommitNewEventsAsync(Account model);

    }
}

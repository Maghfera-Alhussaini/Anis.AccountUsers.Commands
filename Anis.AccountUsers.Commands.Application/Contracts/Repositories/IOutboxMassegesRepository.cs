using Anis.AccountUsers.Commands.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Anis.AccountUsers.Commands.Application.Contracts.Repositories
{
    public interface IOutboxMassegesRepository : IAsyncRepository<OutboxMessage>
    {
    }
}

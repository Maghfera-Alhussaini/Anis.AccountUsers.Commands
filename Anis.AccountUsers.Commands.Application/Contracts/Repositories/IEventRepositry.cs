using Anis.AccountUsers.Commands.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Anis.AccountUsers.Commands.Application.Contracts.Repositories
{
    public interface IEventRepository : IAsyncRepository<Event>
    {
        Task<IEnumerable<Event>> GetAllByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken);
        Task<IEnumerable<Event>> GetAsPaginationAsync(int currentPage = 1, int pageSize = 100);

    }
}

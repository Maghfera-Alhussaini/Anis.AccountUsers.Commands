using Anis.AccountUsers.Commands.Application.Contracts.Repositories;
using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Microsoft.EntityFrameworkCore;
using Anis.AccountUsers.Commands.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anis.AccountUsers.Commands.Domain.Events;

namespace Anis.AccountUsers.Commands.Infra.Persistance.Repositories
{
    public class EventRepository : AsyncRepository<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public EventRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Event>> GetAllByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken)
            => await _appDbContext.Events
                                  .AsNoTracking()
                                  .Where(e => e.AggregateId == aggregateId)
                                  .OrderBy(e => e.Sequence)
                                  .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Event>> GetAsPaginationAsync(int currentPage = 1, int pageSize = 2)
        {
            var skip = (currentPage - 1) * pageSize;

            return await _appDbContext.Events
                                      .AsNoTracking()
                                      .OrderBy(e => e.AggregateId)
                                      .ThenBy(e => e.Sequence)
                                      .Skip(skip)
                                      .Take(pageSize)
                                      .ToListAsync();
        }
    }
}

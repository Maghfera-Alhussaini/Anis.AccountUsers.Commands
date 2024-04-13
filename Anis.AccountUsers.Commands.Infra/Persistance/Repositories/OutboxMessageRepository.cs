using Anis.AccountUsers.Commands.Application.Contracts.Repositories;
using Anis.AccountUsers.Commands.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Infra.Persistance.Repositories
{
    public class OutboxMessageRepository : AsyncRepository<OutboxMessage>, IOutboxMassegesRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public OutboxMessageRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async override Task<IEnumerable<OutboxMessage>> GetAllAsync()
        {
            return await _appDbContext.OutboxMessages
                                      .Include(o => o.Event)
                                      .ToListAsync();
        }
    }
}

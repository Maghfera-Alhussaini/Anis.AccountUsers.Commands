using Anis.AccountUsers.Commands.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Infra.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _appDbContext;

        public UnitOfWork(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private IEventRepository? _events;
        public IEventRepository Events
        {
            get
            {
                if (_events != null)
                    return _events;

                return _events = new EventRepository(_appDbContext);
            }
        }

        private IOutboxMassegesRepository? _outboxMessages;
        public IOutboxMassegesRepository OutboxMessages
        {
            get
            {
                if (_outboxMessages != null)
                    return _outboxMessages;

                return _outboxMessages = new OutboxMessageRepository(_appDbContext);
            }
        }


        public async Task<int> SaveChangesAsync()
          => await _appDbContext.SaveChangesAsync();


        public void Dispose()
        {
            _appDbContext.Dispose();
        }


    }
}

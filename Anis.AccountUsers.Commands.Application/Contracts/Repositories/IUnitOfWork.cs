﻿namespace Anis.AccountUsers.Commands.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IOutboxMassegesRepository OutboxMessages { get; }
        IEventRepository Events { get; }
        Task<int> SaveChangesAsync();
    }
}


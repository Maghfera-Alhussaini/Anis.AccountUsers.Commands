using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Infra.Persistance;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Test.Helpers
{
    public class DbContextHelper
    {
        private readonly IServiceProvider _provider;

        public DbContextHelper(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResult> Query<TResult>(Func<ApplicationDbContext, Task<TResult>> query)
        {
            using var scope = _provider.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await query(appDbContext);
        }

        public async Task<Event> InsertAsync(Event @event)
        {
            using var scope = _provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Events.AddAsync(@event);
            await context.SaveChangesAsync();
            return @event;
        }
    }
}

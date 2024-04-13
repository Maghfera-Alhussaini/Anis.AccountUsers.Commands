using Anis.AccountUsers.Commands.Domain.Entities;
using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Anis.AccountUsers.Commands.Infra.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Infra.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<UserAssignedToAccount, UserAssignedToAccountData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<UserDeletedFromAccount, UserDeletedFromAccountData>());

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

    }
}

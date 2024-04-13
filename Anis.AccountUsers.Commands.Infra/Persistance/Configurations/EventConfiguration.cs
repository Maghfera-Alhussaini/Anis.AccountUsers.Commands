using Anis.AccountUsers.Commands.Domain.Enums;
using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anis.AccountUsers.Commands.Domain.Events;

namespace Anis.AccountUsers.Commands.Infra.Persistance.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(e => e.Type)
                   .HasMaxLength(128)
                   .HasConversion<string>();

            builder.HasDiscriminator(e => e.Type)
                .HasValue<UserAssignedToAccount>(EventType.UserAssignedToAccount)
                .HasValue<UserDeletedFromAccount>(EventType.UserDeletedFromAccount);

        }
    }
}

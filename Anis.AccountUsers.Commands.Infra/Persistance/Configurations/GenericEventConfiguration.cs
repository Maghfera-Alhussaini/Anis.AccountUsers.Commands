using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anis.AccountUsers.Commands.Domain.Events;

namespace Anis.AccountUsers.Commands.Infra.Persistance.Configurations
{
    public class GenericEventConfiguration<TEntity, TData> : IEntityTypeConfiguration<TEntity>
             where TEntity : Event<TData>
             where TData : IEventData
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());


            builder.Property(e => e.Data).HasConversion(
                 v => JsonConvert.SerializeObject(v, jsonSerializerSettings),
                 v => JsonConvert.DeserializeObject<TData>(v)
            ).HasColumnName("Data");
        }
    }
}

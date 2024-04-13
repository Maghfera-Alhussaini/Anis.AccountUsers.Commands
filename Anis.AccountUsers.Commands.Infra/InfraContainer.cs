using Anis.AccountUsers.Commands.Application.Contracts.Repositories;
using Anis.AccountUsers.Commands.Application.Services.BaseServices;
using Anis.AccountUsers.Commands.Application.Services.ServiceBus;
using Anis.AccountUsers.Commands.Infra.Persistance.Repositories;
using Anis.AccountUsers.Commands.Infra.Persistance;
using Anis.AccountUsers.Commands.Infra.Services.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Azure.Messaging.ServiceBus;
using Anis.AccountUsers.Commands.Infra.Services.BaseService;
using Anis.AccountUsers.Commands.Application.Contracts.Services.ServiceBus;

namespace Anis.AccountUsers.Commands.Infra
{
    public static class InfraContainer
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Database")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IServiceBusPublisher, ServiceBusPublisher>();
            services.AddSingleton<IServiceBusEventSender, ServiceBusEventSender>();

            services.AddSingleton(s =>
            {
                return new ServiceBusClient(configuration.GetConnectionString("ServiceBus"));
            });

            services.AddScoped<ICommitEventService, CommitEventService>();

            return services;
        }
    }
}

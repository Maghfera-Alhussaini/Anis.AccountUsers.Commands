using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Application.Services.ServiceBus
{
    public interface IServiceBusPublisher
    {
        void StartPublish();
    }
}

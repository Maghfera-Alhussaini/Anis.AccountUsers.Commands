using Anis.AccountUsers.Commands.Application.Services.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Test.Helpers.FakerServices
{
    public class FakeServiceBusPublisher : IServiceBusPublisher
    {
        public void StartPublish()
        {
            return;
        }
    }
}

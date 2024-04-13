using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using Anis.AccountUsers.Commands.Grpc;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Test.Helpers
{
    public class GrpcClientHelper
    {
        private readonly WebApplicationFactory<Program> _factory;

        public GrpcClientHelper(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        public TResult Send<TResult>(Func<Protos.AccountUsers.AccountUsersClient, TResult> send)
        {
            var client = new Protos.AccountUsers.AccountUsersClient(_factory.CreateGrpcChannel());
            return send(client);
        }

    }
}

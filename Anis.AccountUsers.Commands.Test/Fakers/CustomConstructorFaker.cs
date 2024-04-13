using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Test.Fakers
{
    public class CustomConstructorFaker<T> : Faker<T> where T : class
    {
        public CustomConstructorFaker()
        {
            CustomInstantiator(_ => Initialize());
        }

#pragma warning disable SYSLIB0050 // Type or member is obsolete
        private static T Initialize() =>
            FormatterServices.GetUninitializedObject(typeof(T)) as T ?? throw new TypeLoadException();
#pragma warning restore SYSLIB0050 // Type or member is obsolete
    }
}

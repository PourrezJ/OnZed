using Onsharp.Entities.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed.Entities.Clients
{
    public class ClientFactory : IEntityFactory<Client>
    {
        public Client Create(int id)
        {
            return new Client(id);
        }
    }
}

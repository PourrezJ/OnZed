using Onsharp.Entities.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed.Entities.Clients
{
    public class ClientFactory : IEntityFactory<Survivor>
    {
        public Survivor Create(int id)
        {
            return new Survivor(id);
        }
    }
}

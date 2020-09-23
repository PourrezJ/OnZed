using Onsharp.Entities;
using Onsharp.Entities.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed.Entities.Zeds
{
    class ZedFactory : IEntityFactory<Zed>
    {
        public Zed Create(int id)
        {
            return new Zed(id);
        }
    }
}

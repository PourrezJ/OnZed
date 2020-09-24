using Onsharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed.Entities
{
    public class Survivor : Player
    {
        public PlayerData PlayerData { get; set; }

        public bool Spawned { get; set; }

        public List<Zed> Zeds { get; set; } // List Zed owned

        public Survivor(int id) : base(id)
        {
            Zeds = new List<Zed>();
        }


    }
}

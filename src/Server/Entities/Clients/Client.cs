using Onsharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed.Entities
{
    public class Client : Player
    {
        public PlayerData PlayerData { get; set; }

        public List<Zed> Zeds { get; set; } // List Zed owned

        public Client(int id) : base(id)
        {
            Zeds = new List<Zed>();
        }


    }
}

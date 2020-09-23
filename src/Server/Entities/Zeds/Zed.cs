using OnZed.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed.Entities
{
    public enum ZombieState
    {
        Idle,
        Follow,
        Attack
    }

    public class Zed : Onsharp.Entities.NPC
    {
        public ZombieState ZombieState;

        public Zed(int id) : base(id)
        {
            
        }
    }
}

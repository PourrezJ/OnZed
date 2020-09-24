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

        private Survivor survivorFollowed;
        public Survivor SurvivorFollowed
        {
            get => survivorFollowed;
            set
            {
                survivorFollowed = value;
                Follow(value);
            }
        }

        public Zed(int id) : base(id)
        {
            
        }
    }
}

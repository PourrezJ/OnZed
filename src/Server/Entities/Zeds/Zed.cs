using Onsharp.IO;
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

        public double LastHit;

        private Survivor survivorFollowed;
        public Survivor SurvivorFollowed
        {
            get => survivorFollowed;
            set
            {
                if (value != null)
                {
                    ZombieState = ZombieState.Follow;
                    Follow(value, 320);
                }
                else
                {
                    ZombieState = ZombieState.Idle;
                }
                survivorFollowed = value;

            }
        }

        public Zed(int id) : base(id)
        {
            this.SetPropertyValue("IS_ZOMBIE", true, true);
        }

        public void AttackPlayer(Survivor survivor)
        {
            ZombieState = ZombieState.Attack;


            var timeNow = GameMode.Instance.Runtime.UptimeSeconds;

            if ((timeNow - LastHit) > GameMode.Config.HitDelay)
            {
                this.PlayAnimation(Onsharp.Enums.Animation.Throw);

                LastHit = timeNow;
            }
        }
    }
}

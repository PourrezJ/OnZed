using Onsharp.Enums;
using Onsharp.IO;
using OnZed.Utils;
using OnZed.Utils.Extensions;
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

        private const int ZOMBIEDMGMIN = 10;
        private const int ZOMBIEDMGMAX = 30;
        private const int ZOMBIERUNSPEED = 380;

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
                    MoveTo(GetPosition().Around(50), 60);
                }
                survivorFollowed = value;

            }
        }

        public Zed(int id) : base(id)
        {
            SetPropertyValue("IS_ZOMBIE", true, true);
           
        }

        public void BrainPulse(Survivor survivor)
        {
            if (survivor == null)
            {
                Destroy();
                return;
            }

            if (!IsStreamedFor(survivor) || Dimension != survivor.Dimension || Health == 0)
                return;

            var zpos = GetPosition();

            var ppos = survivor.GetPosition();
            var zdist = ppos.DistanceTo(zpos);

            if (zdist <= GameMode.Config.ZombieMinAgro)
            {
                SurvivorFollowed = survivor;

                float detect = 1000f;
                NoiceDetection(survivor.MoveMode, ref detect);
                Console.WriteLine(detect);
                if (zdist < 1f)
                {
                    AttackPlayer(survivor);
                }
                else if (zdist < detect)
                {
                    if (survivor.IsInVehicle)
                        Follow(survivor.Vehicle, ZOMBIERUNSPEED);
                    else
                        Follow(survivor, ZOMBIERUNSPEED);
                }
            }
            else if (zdist >= GameMode.Config.ZombieMaxAgro)
            {
                SurvivorFollowed = null;
            }
        }

        public void AttackPlayer(Survivor survivor)
        {
            ZombieState = ZombieState.Attack;

            var timeNow = GameMode.Instance.Runtime.UptimeSeconds;

            if (timeNow == 0 ||(timeNow - LastHit) > GameMode.Config.HitDelay)
            {
                this.PlayAnimation(Animation.Throw);

                Onsharp.Threading.Timer.Delay(1000, () =>
                {
                    survivor.Health -= Misc.RandomNumber(ZOMBIEDMGMIN, ZOMBIEDMGMAX);
                    LastHit = GameMode.Instance.Runtime.UptimeSeconds;
                }); 
            }
        }

        public static void NoiceDetection(MoveMode movemode, ref float value)
        {
            switch (movemode)
            {
                case MoveMode.StandingStill:
                    value = value / 4;
                    break;

                case MoveMode.Crouched:
                    value = value / 2;
                    break;

                case MoveMode.Running:
                    value = value * 2;
                    break;
            }
        }
    }
}

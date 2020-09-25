using Onsharp.Entities;
using Onsharp.Enums;
using Onsharp.Events;
using Onsharp.Threading;
using OnZed.Utils.Extensions;
using System.Linq;

namespace OnZed.Entities
{
    public class ZedsManager
    {
        public ZedsManager()
        {
            Timer.Create(() =>
            {
                lock (GameMode.Instance.Server.Players)
                {
                    foreach (Player player in GameMode.Instance.Server.Players)
                    {
                        Survivor client = player as Survivor;

                        if (client == null)
                            continue;

                        if (!client.IsValid)
                            continue;

                        if (!client.Spawned)
                            continue;

                        if (client.Zeds.Count < GameMode.Config.MaxZombiePlayer)
                        {
                            var ppos = client.GetPosition();

                            Zed zed = GameMode.Instance.Server.CreateNPC(ppos.Around(1000), 0) as Zed;
                            client.Zeds.Add(zed);
                        }
                    }
                }
            }, GameMode.Config.ZombieRespawnTime);
            
            Timer.Create(() => 
            {
                if (GameMode.Instance.Server.NPCs.Count >= GameMode.Config.MaxZombieWorld)
                    return;

                lock (GameMode.Instance.Server.NPCs)
                {
                    foreach (Zed zed in GameMode.Instance.Server.NPCs)
                    {
                        if (!zed.IsValid)
                            continue;

                        var zpos = zed.GetPosition();
                        var players = GameMode.Instance.Server.Players.OrderBy(p=> zpos.DistanceTo(p.GetPosition()));

                        zed.BrainPulse((players.Count() > 0) ? (players.ElementAt(0) as Survivor) : null);
                    }
                }
            }, 1500);
        }

        [ServerEvent(EventType.NPCDamage)]
        public void OnNpcDeath(NPC npc, DamageType damageType, double amount)
        {
            if (npc is Zed)
            {
                Zed zed = npc as Zed;

                switch (damageType)
                {
                    case DamageType.Weapon:
                    case DamageType.Vehicle:
                    case DamageType.Explosion:
                        zed.LastHit = GameMode.Instance.Runtime.UptimeSeconds;
                        break;
                }
            }
        }

        [ServerEvent(EventType.NPCDeath)]
        public void OnNpcDeath(NPC npc)
        {
            if (npc is Zed)
            {
                Zed zed = npc as Zed;

                if (zed.SurvivorFollowed != null)
                {
                    Survivor survivor = zed.SurvivorFollowed;

                    if (survivor.Zeds.Contains(zed))
                    {
                        survivor.Zeds.Remove(zed);
                        zed.SetRagdoll(true);
                        
                        Timer.Create(() => zed.Destroy(), 2000);
                    }
                }
            }
        }
    }
}

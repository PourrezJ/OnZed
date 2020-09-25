using Onsharp.Entities;
using Onsharp.Events;
using Onsharp.Threading;
using Onsharp.World;
using OnZed.Utils;
using OnZed.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

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

                        if (!client.Spawned)
                            continue;

                        var ppos = player.GetPosition();

                        if (client.Zeds.Count < GameMode.Config.MaxZombiePlayer)
                        {
                            Zed zed = GameMode.Instance.Server.CreateNPC(ppos.Around(1000), 0) as Zed;
                            client.Zeds.Add(zed);
                        }

                        lock (client.Zeds)
                        {
                            foreach (var zed in client.Zeds.ToList()) // Création d'une liste temporaire pour check les zeds à supprimer
                            {
                                if (!zed.IsStreamedFor(client) || zed.Dimension != client.Dimension || zed.Health == 0)
                                    continue;

                                var zpos = zed.GetPosition();
                                var zdist = ppos.DistanceTo(zpos);

                                if (zdist <= GameMode.Config.ZombieMinAgro)
                                {
                                    zed.SurvivorFollowed = client;

                                    if (zdist < 1f)
                                    {
                                        zed.AttackPlayer(client);
                                    }
                                    else if (zdist < 1000f)
                                    {
                                        zed.Follow(client, 320);
                                    }
                                }
                                else if (zdist >= GameMode.Config.MaxZombiePlayer)
                                {
                                    zed.ZombieState = ZombieState.Idle;
                                    zed.MoveTo(zpos.Around(5), 60);
                                    zed.SurvivorFollowed = null;
                                }
                            }
                        }
                    }
                }
            }, 1500);
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
                        //zed.Destroy();
                    }
                }
            }
        }
    }
}

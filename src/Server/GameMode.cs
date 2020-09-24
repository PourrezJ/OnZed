using Onsharp.Commands;
using Onsharp.Events;
using Onsharp.Plugins;
using OnZed.Entities;
using OnZed.Entities.Clients;
using OnZed.Entities.Zeds;
using OnZed.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OnZed
{
    [PluginMeta("OnZed", "OnZed", "1.0", "Djoe45", IsDebug = true)]
    public class GameMode : Plugin
    {
        public static GameMode Instance { get; private set; }

        public static OnZedConfig Config { get; private set; }

        public GameMode()
        {
            if (Instance != null)
                return;

            Instance = this;

        }

        public override void OnStart()
        {
            try
            {
                Logger.Info("Starting OnZed Resource!");

                TaskManager.Initialize();

                Server.OverrideEntityFactory(new ClientFactory());
                Server.OverrideEntityFactory(new ZedFactory());

                Server.RegisterServerEvents(new PlayerManager());
                Server.RegisterServerEvents(new ZedsManager());

                Logger.Info("Loading server configuration.");
                Config = Data.Config<OnZedConfig>();

                Logger.Info("Loading MongoDB database");
                if (!Database.MongoDb.Init())
                {
                    Logger.Warn("Error for loading MongoDB!\nApplication quit in few seconds...");
                    Thread.Sleep(15000);
                    Environment.Exit(0);
                    return;
                }

                Logger.Info("Loading all players from database.");
                PlayerManager.LoadAllPlayerDatabase();

                Logger.Info("Loading OnZed End!");
            }
            catch(Exception ex)
            {
                Logger.Error(ex, "");
            }
        }

        public override void OnStop()
        {
            Logger.Info("OnStop Called");
        }


        #region ServerEvents

        [ServerEvent(EventType.ConsoleInput)]
        public void OnConsoleInput(string text)
        {
            string[] args = text.Split(' ');

            if (args.Length <= 1)
                return;

            string command = args[0];
            string package = args[1];

            if (string.IsNullOrEmpty(package))
                return;

            switch (command)
            {
                case "restart":

                    if (!Runtime.IsPackageStarted(package))
                        Runtime.StartPackage(package);
                    else
                    {
                        Runtime.StopPackage(package);

                        Misc.Delay(500, () => Runtime.StartPackage(package));
                    }

                    Logger.Info(package + " restarted");

                    break;

                case "stop":
                    Runtime.StopPackage(package);
                    break;

                case "start":
                    Runtime.StartPackage(package);
                    break;

                default:

                    break;
            }
        }

        [ServerEvent(EventType.GameTick)]
        public void OnTick(double delta)
        {
            TaskManager.Pulse();
        }
    }
    #endregion
}

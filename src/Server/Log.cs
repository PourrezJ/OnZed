using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed
{
    public static class Log
    {
        public static void Info(string log, params object[] parameters)
        {
            GameMode.Instance.Logger.Info(log, parameters);
        }

        public static void Debug(string log, params object[] parameters)
        {
            GameMode.Instance.Logger.Debug(log, parameters);
        }

        public static void Error(Exception exection, string log, params object[] parameters)
        {
            GameMode.Instance.Logger.Error(exection, log, parameters);
        }

        public static void Fatal(string log, params object[] parameters)
        {
            GameMode.Instance.Logger.Fatal(log, parameters);
        }

        public static void Warn(string log, params object[] parameters)
        {
            GameMode.Instance.Logger.Warn(log, parameters);
        }
    }
}

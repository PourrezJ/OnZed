using Onsharp.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed.Utils.Extensions
{
    public static class VectorExtension
    {
        private static Random Instance = new Random();

        public static Vector Around(this Vector vector3, float distance)
        {
            var round = RandomXY();
            return vector3 + new Vector(round.X * distance, round.Y * distance, round.Z * distance);
        }

        public static Vector RandomXY()
        {
            Vector v = new Vector();
            double radian = Instance.NextDouble() * 2 * System.Math.PI;

            v.X = (float)System.Math.Cos(radian);
            v.Y = (float)System.Math.Sin(radian);
            v.Normalize();

            return v;
        }
    }
}

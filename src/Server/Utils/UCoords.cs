
using Onsharp.World;

namespace OnZed.Utils
{
    public class UCoords
    {
        public UCoords(double x, double y, double z, float heading)
        {
            X = x;
            Y = y;
            Z = z;
            Heading = heading;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public float Heading { get; set; }

        public Vector ToVector3()
        {
            return new Vector(X, Y, Z);
        }

        public double DistanceTo(Vector position)
        {
            return (position - new Vector(X, Y, Z)).Length();
        }

        public double DistanceTo2D(Vector position)
        {
            return (new Vector(position.X, position.Y, 0) - new Vector(X, Y, 0.0f)).Length();
        }

        public void SetUcoord(Vector pos, float heading)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
            Heading = heading;
        }

        public override string ToString()
        {
            return $"X:{X} Y:{Y} Z:{Z} Heading:{Heading}";
        }
    }
}

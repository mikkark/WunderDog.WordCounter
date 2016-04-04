using System;
using System.Collections.Generic;
using System.Linq;

namespace WunderDog.WordFinder
{
    public struct CustomPoint
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public CustomPoint(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public int X => _x;

        public int Y => _y;

        public int Z => _z;

        public override bool Equals(object obj)
        {
            CustomPoint other = (CustomPoint)obj;

            if (other.X == X && other.Y == Y && other.Z == Z)
            {
                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();
            hash = (hash * 7) + Z.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }

        public static int operator -(CustomPoint c1, CustomPoint c2)
        {
            int[] distances = new int[3];

            distances[0] = Math.Abs(c1.X - c2.X);
            distances[1] = Math.Abs(c1.Y - c2.Y);
            distances[2] = Math.Abs(c1.Z - c2.Z);

            return distances.Max();
        }

        public bool IsNeighbourOf(CustomPoint other)
        {
            if (Math.Abs(other.Z - Z) > 1)
            {
                return false;
            }

            if (Math.Abs(other.Z - Z) == 1 && other.Y == Y && other.X == X)
            {
                return true;
            }

            if (other.X == X && Math.Abs(other.Y - Y) == 1)
            {
                return true;
            }

            if (other.Y == Y && Math.Abs(other.X - X) == 1)
            {
                return true;
            }

            if (Math.Abs(other.X - X) == 1 && Math.Abs(other.Y - Y) == 1)
            {
                return true;
            }

            return false;
        }
    }
}
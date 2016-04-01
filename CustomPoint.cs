using System;
using System.Collections.Generic;

namespace WunderDog.WordFinder
{
    internal struct CustomPoint
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

        public bool IsNeighbourOf(CustomPoint other)
        {
            if (Math.Abs(other.Z - Z) > 1)
            {
                return false;
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
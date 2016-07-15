using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// ReSharper disable UnusedMember.Global

namespace MissionControl
{
	public struct BlockPos : IEquatable<BlockPos>
	{
		public static BlockPos Zero => new BlockPos();
		public static BlockPos One => new BlockPos(1);

		public static BlockPos UnitX => new BlockPos(1, 0, 0);
		public static BlockPos UnitY => new BlockPos(0, 1, 0);
		public static BlockPos UnitZ => new BlockPos(0, 0, 1);

		public int X
		{ get; }

		public int Y
		{ get; }

		public int Z
		{ get; }

		public double Abs => Math.Sqrt(X * X + Y * Y + Z * Z);

		public double Theta => Math.Atan2(X, Y);

		public BlockPos(int val) : this(val, val, val)
		{ }

		public BlockPos(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public BlockPos WithX(int x)
		{
			return new BlockPos(x, Y, Z);
		}

		public BlockPos WithY(int y)
		{
			return new BlockPos(X, y, Z);
		}

		public BlockPos WithZ(int z)
		{
			return new BlockPos(X, Y, z);
		}

		public bool Equals(BlockPos other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		public override int GetHashCode()
		{
			return X ^ Y ^ Z;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			try
			{
				BlockPos other = (BlockPos)obj;

				return Equals(other);
			}
			catch (InvalidCastException)
			{
				return false;
			}
		}

		public override string ToString()
		{
			return $"({X}, {Y}, {Z})";
		}

		public string ToString(string format)
		{
			return "(" + X.ToString(format) + ", " + Y.ToString(format) + ", " + Z.ToString(format) + ")";
		}

		public double Distance(BlockPos other)
		{
			double dx = X - other.X;
			double dy = Y - other.Y;
			double dz = Z - other.Z;

			return Math.Sqrt(dx * dx + dy * dy + dz * dz);
		}

		public static bool operator ==(BlockPos a, BlockPos b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(BlockPos a, BlockPos b)
		{
			return !a.Equals(b);
		}

		public static BlockPos operator +(BlockPos a, BlockPos b)
		{
			return new BlockPos(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static BlockPos operator -(BlockPos a, BlockPos b)
		{
			return new BlockPos(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public static BlockPos operator *(BlockPos val, int k)
		{
			return new BlockPos(val.X * k, val.Y * k, val.Z * k);
		}

		public static BlockPos operator /(BlockPos val, int k)
		{
			return new BlockPos(val.X / k, val.Y / k, val.Z / k);
		}

		public static BlockPos operator -(BlockPos val)
		{
			return new BlockPos(-val.X, -val.Y, -val.Z);
		}

		public static implicit operator Vector3(BlockPos pos)
		{
			return new Vector3(pos.X, pos.Y, pos.Z);
		}
	}
}
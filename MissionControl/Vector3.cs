using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable UnusedMember.Global
namespace MissionControl
{
	public struct Vector3 : IEquatable<Vector3>
	{
		public static Vector3 Zero => new Vector3();
		public static Vector3 One => new Vector3(1, 1, 1);

		public static Vector3 UnitX => new Vector3(1, 0, 0);
		public static Vector3 UnitY => new Vector3(0, 1, 0);
		public static Vector3 UnitZ => new Vector3(0, 0, 1);


		public double X
		{ get; }

		public double Y
		{ get; }

		public double Z
		{ get; }

		public double Abs => Math.Sqrt(X * X + Y * Y + Z * Z);

		public double Theta => Math.Atan2(X, Y);

		public Vector3(double amount)
		{
			X = amount;
			Y = amount;
			Z = amount;
		}

		public Vector3(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3 WithX(double x)
		{
			return new Vector3(x, Y, Z);
		}

		public Vector3 WithY(double y)
		{
			return new Vector3(X, y, Z);
		}

		public Vector3 WithZ(double z)
		{
			return new Vector3(X, Y, z);
		}

		public bool Equals(Vector3 other)
		{
			return other.X == X && other.Y == Y && other.Z == Z;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			try
			{
				Vector3 other = (Vector3)obj;

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

		public double Distance(Vector3 other)
		{
			double dx = X - other.X;
			double dy = Y - other.Y;
			double dz = Z - other.Z;

			return Math.Sqrt(dx * dx + dy * dy + dz * dz);
		}

		public static bool operator ==(Vector3 a, Vector3 b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Vector3 a, Vector3 b)
		{
			return !a.Equals(b);
		}

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public static Vector3 operator *(Vector3 val, double k)
		{
			return new Vector3(val.X * k, val.Y * k, val.Z * k);
		}

		public static Vector3 operator /(Vector3 val, double k)
		{
			return new Vector3(val.X / k, val.Y / k, val.Z / k);
		}

		public static Vector3 operator -(Vector3 val)
		{
			return new Vector3(-val.X, -val.Y, -val.Z);
		}

		public static explicit operator BlockPos(Vector3 v)
		{
			return new BlockPos((int)v.X, (int)v.Y, (int)v.Z);
		}
	}
}
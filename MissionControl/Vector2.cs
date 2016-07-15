using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable UnusedMember.Global
namespace MissionControl
{
	public struct Vector2 : IEquatable<Vector2>
	{
		public static Vector2 Zero => new Vector2();
		public static Vector2 One => new Vector2(1, 1);

		public static Vector2 UnitX => new Vector2(1, 0);

		public static Vector2 UnitY => new Vector2(0, 1);


		public double X
		{ get; }

		public double Y
		{ get; }

		public double Abs => Math.Sqrt(X * X + Y * Y);

		public double Theta => Math.Atan2(X, Y);

		public Vector2(double amount)
		{
			X = amount;
			Y = amount;
		}

		public Vector2(double x, double y)
		{
			X = x;
			Y = y;
		}

		public Vector2 WithX(double x)
		{
			return new Vector2(x, Y);
		}

		public Vector2 WithY(double y)
		{
			return new Vector2(X, y);
		}

		public bool Equals(Vector2 other)
		{
			return other.X == X && other.Y == Y;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			try
			{
				Vector2 other = (Vector2)obj;

				return Equals(other);
			}
			catch (InvalidCastException)
			{
				return false;
			}
		}

		public override string ToString()
		{
			return $"({X}, {Y})";
		}

		public string ToString(string format)
		{
			return "(" + X.ToString(format) + ", " + Y.ToString(format) + ")";
		}

		public double Distance(Vector2 other)
		{
			double dx = X - other.X;
			double dy = Y - other.Y;

			return Math.Sqrt(dx * dx + dy * dy);
		}

		public static bool operator==(Vector2 a, Vector2 b)
		{
			return a.Equals(b);
		}

		public static bool operator!=(Vector2 a, Vector2 b)
		{
			return !a.Equals(b);
		}

		public static Vector2 operator+(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X + b.X, a.Y + b.Y);
		}

		public static Vector2 operator-(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X - b.X, a.Y - b.Y);
		}

		public static Vector2 operator*(Vector2 val, double k)
		{
			return new Vector2(val.X * k, val.Y * k);
		}

		public static Vector2 operator/(Vector2 val, double k)
		{
			return new Vector2(val.X / k, val.Y / k);
		}

		public static Vector2 operator-(Vector2 val)
		{
			return new Vector2(-val.X, -val.Y);
		}
	}
}
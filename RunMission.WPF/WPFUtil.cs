using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;

namespace RunMission.WPF
{
	public static class WPFUtil
	{
		public static readonly Color DARKER_GRAY = new Color { A = 255, R = 60, G = 60, B = 60 };

		[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		public static Color ToWPF(this ConsoleColor con)
		{
			switch (con)
			{
			case ConsoleColor.Black:
				return Colors.Black;
			case ConsoleColor.DarkBlue:
				return Colors.Blue;
			case ConsoleColor.DarkGreen:
				return Colors.DarkGreen;
			case ConsoleColor.DarkCyan:
				return Colors.DarkCyan;
			case ConsoleColor.DarkRed:
				return Colors.DarkRed;
			case ConsoleColor.DarkMagenta:
				return Colors.DarkViolet;
			case ConsoleColor.DarkYellow:
				return Colors.DarkOrange;
			case ConsoleColor.Gray:
				return Colors.DarkGray;
			case ConsoleColor.DarkGray:
				return DARKER_GRAY;
			case ConsoleColor.Blue:
				return Colors.DodgerBlue;
			case ConsoleColor.Green:
				return Colors.Lime;
			case ConsoleColor.Cyan:
				return Colors.Cyan;
			case ConsoleColor.Red:
				return Colors.Red;
			case ConsoleColor.Magenta:
				return Colors.Magenta;
			case ConsoleColor.Yellow:
				return Colors.Yellow;
			case ConsoleColor.White:
				return Colors.White;
			default:
				throw new ArgumentOutOfRangeException(nameof(con), con, null);
			}
		}

		public static SolidColorBrush ToBrush(this Color col)
		{
			return new SolidColorBrush(col);
		}

		public static SolidColorBrush ToBrush(this ConsoleColor con)
		{
			return con.ToWPF().ToBrush();
		}

		public static void BringExternalWindowToFront(string title)
		{
			// Get a handle to the application.
			IntPtr handle = FindWindow(null, title);

			// Verify that it is a running process.
			if (handle == IntPtr.Zero)
			{
				return;
			}

			// Make it the foreground application
			SetForegroundWindow(handle);
		}
	}
}
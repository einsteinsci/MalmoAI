

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControl
{
	public static class Util
	{
		public static double NextDouble(this Random rand, double min, double max)
		{
			return rand.NextDouble() * (max - min) + min;
		}

		public static double NextDouble(this Random rand, double max)
		{
			return rand.NextDouble() * max;
		}
	}
}
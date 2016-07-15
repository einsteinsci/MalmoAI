using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using Newtonsoft.Json;

using UltimateUtil;

namespace MissionControl.Observations
{
	[UsedImplicitly]
	[JsonObject(MemberSerialization.OptIn)]
	public class StatusObservations
	{
		[JsonProperty]
		public long DistanceTravelled
		{ get; set; }

		[JsonProperty]
		public long TimeAlive
		{ get; set; }

		public TimeSpan TimeAliveSpan => TimeSpan.FromSeconds(TimeAlive / 20.0);

		[JsonProperty]
		public int MobsKilled
		{ get; set; }

		[JsonProperty]
		public int DamageTaken
		{ get; set; }

		[JsonProperty]
		public double Life
		{ get; set; }

		[JsonProperty]
		public double Food
		{ get; set; }

		[JsonProperty]
		public double XP
		{ get; set; }

		[JsonProperty]
		public bool IsAlive
		{ get; set; }

		[JsonProperty]
		public double XPos
		{ get; set; }

		[JsonProperty]
		public double YPos
		{ get; set; }

		[JsonProperty]
		public double ZPos
		{ get; set; }

		public Vector3 Pos => new Vector3(XPos, YPos, ZPos);

		[JsonProperty]
		public double Pitch
		{ get; set; }

		[JsonProperty]
		public double Yaw
		{ get; set; }

		public Vector2 Aim => new Vector2(Yaw, Pitch);

		public override string ToString()
		{
			string res = "";

			res += "Dist: {0}\n".Fmt(DistanceTravelled);
			res += "Time Alive: {0:mm\\:ss}\n".Fmt(TimeAliveSpan);
			res += "Mobs Killed: {0}\n".Fmt(MobsKilled);
			res += "Damage Taken: {0}\n".Fmt(DamageTaken);
			res += "Life: {0:F1}\n".Fmt(Life);
			res += "Food: {0:F1}\n".Fmt(Food);
			res += "XP: {0:F2}\n".Fmt(XP);
			res += "IsAlive: {0}\n".Fmt(IsAlive);
			res += "Pos: {0}\n".Fmt(Pos.ToString("F2"));
			res += "Aim: " + Aim.ToString("F2");

			return res;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Malmo;

using MissionControl;
using MissionControl.Observations;

using Newtonsoft.Json;

using UltimateUtil;
using UltimateUtil.UserInteraction;

namespace RunMission.WPF
{
	public sealed class AdHocAI : AIBase
	{
		public override int FrameTime => 50;

		public StatusObservations Status
		{ get; private set; }

		private long _agentFrame;

		public AdHocAI(AgentHost agent) : base(agent)
		{ }

		public override string GetDebugString()
		{
			string res = "Agent Frame: " + _agentFrame + "\n";

			if (Status != null)
			{
				res += Status.ToString();
			}

			return res;
		}

		public override void InitializeMission(string xml)
		{
			Mission = new MissionSpec(xml, false);
		}

		public override void FirstActions()
		{
			_agentFrame = 0;
			VersatileIO.WriteLine("Ad-Hoc agent started.", ConsoleColor.Green);
		}

		public override void Update()
		{
			_agentFrame++;

			if (!World.observations.IsNullOrEmpty())
			{
				foreach (TimestampedString tss in World.observations)
				{
					string json = tss.text;
					try
					{
						Status = JsonConvert.DeserializeObject<StatusObservations>(json);
					}
					catch (JsonException ex)
					{
						VersatileIO.WriteLine(ex.MakeExceptionInfo());
					}
				}
			}
		}
	}
}
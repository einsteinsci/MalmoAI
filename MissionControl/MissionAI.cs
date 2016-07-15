using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Research.Malmo;

using MissionControl.Observations;

using Newtonsoft.Json;

using UltimateUtil;
using UltimateUtil.Logging;
using UltimateUtil.UserInteraction;

namespace MissionControl
{
	public sealed class MissionAI : AIBase
	{
		public const float TIME_LIMIT = 20.0f;

		private long _agentFrame;

		public StatusObservations Status
		{ get; private set; }

		public override int FrameTime => 50;

		public MissionAI(AgentHost agent) : base(agent)
		{ }

		public override string GetDebugString()
		{
			string res = "";

			res += "Agent Frame: " + _agentFrame + "\n";
			if (Status != null)
			{
				res += Status.ToString();
			}

			return res;
		}

		public override void InitializeMission(string xml)
		{
			Mission = new MissionSpec(xml, false);
			Mission.timeLimitInSeconds(TIME_LIMIT);
			Mission.rewardForReachingPosition(19.5f, 2.0f, 19.5f, 100.0f, 1.1f);
			//Mission.forceWorldReset();
		}

		public override void FirstActions()
		{
			base.FirstActions();

			_agentFrame = 0;
			ChatAgentCommands.SendChat("/difficulty peaceful");
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

			ContinuousAgentCommands.Move(1);
			ContinuousAgentCommands.Turn(Rand.NextDouble(-20, 20));
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Malmo;

using MissionControl;

using UltimateUtil.Logging;

namespace RunMission.WPF
{
	public class AdHocAI : MissionAI
	{
		public override int FrameTime => 50;

		public AdHocAI(AgentHost agent) : base(agent)
		{ }

		public override void InitializeMission(string xml)
		{
			Mission = new MissionSpec(xml, false);
		}

		public override void FirstActions()
		{
			Logger.LogSuccess("Ad-Hoc agent started.");
		}

		public override void Update()
		{
			agentFrame++;
		}
	}
}
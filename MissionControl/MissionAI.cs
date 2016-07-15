using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Research.Malmo;

namespace MissionControl
{
	public class MissionAI
	{
		public const float TIME_LIMIT = 20.0f;

		public AgentHost Agent
		{ get; }

		public MissionSpec Mission
		{ get; protected set; }

		public WorldState World
		{ get; set; }

		public Random Rand
		{ get; }

		public virtual int FrameTime => 200;

		protected long agentFrame;

		public MissionAI(AgentHost agent)
		{
			Agent = agent;
			Rand = new Random();
		}

		public virtual string GetDebugString()
		{
			string res = "";

			res += "Agent Frame: " + agentFrame;

			return res;
		}

		public virtual void InitializeMission(string xml)
		{
			Mission = new MissionSpec(xml, false);
			Mission.timeLimitInSeconds(TIME_LIMIT);
			Mission.rewardForReachingPosition(19.5f, 2.0f, 19.5f, 100.0f, 1.1f);
			//Mission.forceWorldReset();
		}

		public virtual void FirstActions()
		{
			agentFrame = 0;
			World = Agent.getWorldState();

			Mission.allowAllAbsoluteMovementCommands();
			Agent.sendCommand("setPitch 40");
			
			Mission.allowAllChatCommands();
			//Agent.sendCommand("chat /tp @p ~ ~1 ~ 0 0");

			Mission.allowAllContinuousMovementCommands();

			Thread.Sleep(1000);
		}

		public virtual void Update()
		{
			agentFrame++;

			ContinuousAgentCommands.Move(1);
			ContinuousAgentCommands.Turn(Rand.NextDouble(-20, 20));
		}
	}
}
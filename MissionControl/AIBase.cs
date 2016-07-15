using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Malmo;

using MissionControl.Observations;

namespace MissionControl
{
	public abstract class AIBase
	{
		public AgentHost Agent
		{ get; }

		public MissionSpec Mission
		{ get; protected set; }

		public WorldState World
		{ get; set; }

		public Random Rand
		{ get; }

		public abstract int FrameTime
		{ get; }

		protected AIBase(AgentHost agent)
		{
			Agent = agent;
			Rand = new Random();
		}

		public virtual string GetDebugString()
		{
			return "";
		}

		public abstract void InitializeMission(string xml);

		public virtual void FirstActions()
		{
			World = Agent.getWorldState();
		}

		public abstract void Update();
	}
}
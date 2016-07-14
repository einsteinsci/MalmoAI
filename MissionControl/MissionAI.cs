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
		public AgentHost Agent
		{ get; }

		public MissionSpec Mission
		{ get; }

		public WorldState World
		{ get; set; }

		public Random Rand
		{ get; }

		public MissionAI(AgentHost agent, MissionSpec mission)
		{
			Agent = agent;
			Mission = mission;
			Rand = new Random();
		}

		public void Initialize()
		{
			World = Agent.getWorldState();

			Mission.allowAllAbsoluteMovementCommands();
			Agent.sendCommand("setPitch 40");
			
			Mission.allowAllChatCommands();
			//Agent.sendCommand("chat /tp @p ~ ~1 ~ 0 0");

			Mission.allowAllContinuousMovementCommands();

			Thread.Sleep(1000);
		}

		public void Update()
		{
			ContinuousAgentCommands.Move(1);
			ContinuousAgentCommands.Turn(Rand.NextDouble(-20, 20));

			Thread.Sleep(200);

			List<string> observations = World.observations.ToList().ConvertAll(tss => tss.text);
		}
	}
}
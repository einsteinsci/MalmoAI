using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Malmo;

namespace MissionControl
{
	public static class ChatAgentCommands
	{
		public static AgentHost Agent
		{ get; private set; }

		public static void Initialize(AgentHost agent)
		{
			Agent = agent;
		}

		public static void SendChat(string text)
		{
			Agent.sendCommand("chat " + text);
		}
	}
}
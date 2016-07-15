using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RunMission.WPF.View;

using UltimateUtil.Logging;
using UltimateUtil.UserInteraction;

namespace RunMission.WPF
{
	public class CommandHandler
	{
		public AgentView View
		{ get; }

		public CommandHandler(AgentView view)
		{
			View = view;
		}

		public void RunCommand(string cmdLine)
		{
			string lower = cmdLine.ToLower();

			if (lower == "start")
			{
				if (!View.Runner.IsAgentRunning)
				{
					View.AdHoc = true;
					View.StartRunner();
				}
				else
				{
					Logger.LogWarning("Ad-Hoc agent already running.");
				}
				return;
			}

			if (lower == "stop")
			{
				if (View.Runner.IsAgentRunning)
				{
					View.AdHoc = false;
					View.StopRunner();
				}
				else
				{
					Logger.LogWarning("Ad-Hoc agent not running.");
				}
				return;
			}

			if (View.Runner.IsAgentRunning)
			{
				View.Runner.Agent.sendCommand(cmdLine);
				Logger.LogInfo("Sent command '{0}'.", cmdLine);
			}
		}
	}
}
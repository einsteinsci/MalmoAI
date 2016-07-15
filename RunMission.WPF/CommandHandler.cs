using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MissionControl;

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
					VersatileIO.WriteLine("Ad-Hoc agent already running.", ConsoleColor.Yellow);
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
					VersatileIO.WriteLine("Ad-Hoc agent not running.", ConsoleColor.Yellow);
				}
				return;
			}

			if (lower == "crash")
			{
				try
				{
					throw new Exception("THIS IS NOT A ERROR.");
				}
				catch (Exception ex)
				{
					VersatileIO.WriteLine(ex.MakeCrashReport(null, null), ConsoleColor.Red);
				}
				return;
			}

			if (lower == "throw")
			{
				try
				{
					throw new Exception("THIS IS NOT A ERROR.");
				}
				catch (Exception ex)
				{
					VersatileIO.WriteLine(ex.MakeExceptionInfo(), ConsoleColor.Red);
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
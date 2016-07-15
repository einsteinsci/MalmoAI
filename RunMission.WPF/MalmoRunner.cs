using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Research.Malmo;

using MissionControl;

using RunMission.WPF.View;

using UltimateUtil.Logging;
using UltimateUtil.UserInteraction;

namespace RunMission.WPF
{
	public class MalmoRunner
	{
		public AgentView View
		{ get; }

		public bool IsAgentRunning
		{ get; private set; }

		public bool IsCancelling
		{ get; private set; }

		public AgentHost Agent
		{ get; private set; }

		public MalmoRunner(AgentView view)
		{
			View = view;
		}

		public void Cancel()
		{
			IsCancelling = true;
		}

		public void OnRunExit()
		{
			IsAgentRunning = false;
			VersatileIO.WriteLine();
			VersatileIO.Write("mission> ", ConsoleColor.Blue);

			View.Dispatcher.Invoke(() => {
				View.RunAgentBtn.IsEnabled = true;
				View.StopAgentBtn.IsEnabled = false;
			});

			Agent.Dispose();
		}

		public void Run(bool adhoc, params string[] args)
		{
			IsAgentRunning = true;

			View.Dispatcher.Invoke(() => {
				View.RunAgentBtn.IsEnabled = false;
				View.StopAgentBtn.IsEnabled = true;
			});

			string xml = "";
			if (File.Exists("MissionSetup.xml"))
			{
				xml = File.ReadAllText("MissionSetup.xml");
			}
			
			try
			{
				Agent = new AgentHost();
				Agent.parse(new StringVector(args));
			}
			catch (Exception ex)
			{
				VersatileIO.WriteLine(ex.MakeExceptionInfo(), ConsoleColor.Red);
				IsAgentRunning = false;
				OnRunExit();
				return;
			}

			if (args.ToList().Exists(s => s.ToLower() == "help"))
			{
				VersatileIO.WriteLine(Agent.getUsage(), ConsoleColor.DarkCyan);
				OnRunExit();
				return;
			}

			AIBase ai;
			try
			{
				ai = adhoc ? new AdHocAI(Agent) as AIBase : new MissionAI(Agent);
				ai.InitializeMission(xml);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.MakeExceptionInfo());
				OnRunExit();
				return;
			}

			MissionRecordSpec record = new MissionRecordSpec("./saved_data.tgz");
			record.recordCommands();
			record.recordMP4(60, 400000);
			record.recordRewards();
			record.recordObservations();
			
			try
			{
				Logger.LogInfo("Starting Mission...");
				Agent.startMission(ai.Mission, record);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.MakeCrashReport(ai.Mission, Agent.peekWorldState()));
				OnRunExit();
				return;
			}

			VersatileIO.Write("Waiting for mission to start...", ConsoleColor.Yellow);
			do
			{
				VersatileIO.Write(".");
				Thread.Sleep(500);
				ai.World = Agent.getWorldState();

				foreach (TimestampedString tss in ai.World.errors)
				{
					Logger.LogError("World Error: " + tss.text);
				}

				if (IsCancelling)
				{
					IsCancelling = false;
					VersatileIO.WriteLine("Mission setup canceled.", ConsoleColor.Red);
					OnRunExit();
					return;
				}
			} while (!ai.World.is_mission_running);

			VersatileIO.WriteLine();

			ContinuousAgentCommands.Initialize(Agent);
			AbsoluteAgentCommands.Initialize(Agent);
			ChatAgentCommands.Initialize(Agent);

			ai.FirstActions();
			
			while (ai.World.is_mission_running)
			{
				Agent.sendCommand("");
				ai.Update();

				View.Dispatcher.Invoke(() => View.DebugTxt.Text = ai.GetDebugString());

				Thread.Sleep(ai.FrameTime);
				ai.World = Agent.getWorldState();

				foreach (TimestampedReward tsr in ai.World.rewards)
				{
					VersatileIO.WriteLine("Total Rewards: " + tsr.getValue(), ConsoleColor.Green);
				}

				foreach (TimestampedString tss in ai.World.errors)
				{
					VersatileIO.WriteLine("World Error: " + tss.text, ConsoleColor.Red);
				}

				if (IsCancelling)
				{
					IsCancelling = false;
					VersatileIO.WriteLine("Mission stopped.", ConsoleColor.Red);
					OnRunExit();
					return;
				}
			}
			
			VersatileIO.WriteLine("Mission complete! (Return to base.)", ConsoleColor.Green);

			OnRunExit();
		}
	}
}
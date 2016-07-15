// --------------------------------------------------------------------------------------------------
//  Copyright (c) 2016 Microsoft Corporation
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
//  associated documentation files (the "Software"), to deal in the Software without restriction,
//  including without limitation the rights to use, copy, modify, merge, publish, distribute,
//  sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in all copies or
//  substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
//  NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Research.Malmo;
using MissionControl;

using UltimateUtil;

//using UltimateUtil;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once CheckNamespace
public class Program
{
	public const float TIME_LIMIT = 20.0f;

	public static void Main()
	{
		string xml = "";
		if (File.Exists("MissionSetup.xml"))
		{
			xml = File.ReadAllText("MissionSetup.xml");
		}

		AgentHost agentHost = new AgentHost();
		try
		{
			List<string> args = Environment.GetCommandLineArgs().ToList();
			agentHost.parse(new StringVector(args));
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine("ERROR: {0}", ex.Message);
			Console.Error.WriteLine(agentHost.getUsage());
			Console.ReadKey();
			Environment.Exit(1);
		}

		if (agentHost.receivedArgument("help"))
		{
			Console.Error.WriteLine(agentHost.getUsage());
			Console.ReadKey();
			Environment.Exit(0);
		}

		MissionAI ai = new MissionAI(agentHost);
		
		ai.InitializeMission(xml);
		MissionSpec mission = ai.Mission;

		MissionRecordSpec missionRecord = new MissionRecordSpec("./saved_data.tgz");
		missionRecord.recordCommands();
		missionRecord.recordMP4(60, 400000);
		missionRecord.recordRewards();
		missionRecord.recordObservations();

		try
		{
			agentHost.startMission(mission, missionRecord);
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine("Error starting mission: {0}", ex.Message);
			Console.ReadKey();
			Environment.Exit(1);
		}

		Console.WriteLine("Waiting for the mission to start");
		do
		{
			Console.Write(".");
			Thread.Sleep(100);
			ai.World = agentHost.getWorldState();

			foreach (TimestampedString error in ai.World.errors)
			{
				Console.Error.WriteLine("Error: {0}", error.text);
			}
		} while (!ai.World.is_mission_running);

		Console.WriteLine();

		ContinuousAgentCommands.Initialize(agentHost);
		AbsoluteAgentCommands.Initialize(agentHost);
		ChatAgentCommands.Initialize(agentHost);

		// One-Time init AI:
		ai.FirstActions();
		
		// main loop:
		while (ai.World.is_mission_running)
		{
			agentHost.sendCommand("");
			ai.Update();
			ai.World = agentHost.getWorldState();

			//Console.WriteLine("Frames: {0}, Observations: {1}, Rewards: {2}", worldState.number_of_video_frames_since_last_state,
			//	worldState.number_of_observations_since_last_state, worldState.number_of_rewards_since_last_state);

			foreach (TimestampedReward reward in ai.World.rewards)
			{
				Console.Error.WriteLine($"Summed reward: {reward.getValue()}");
			}
			foreach (TimestampedString error in ai.World.errors)
			{
				Console.Error.WriteLine($"Error: {error.text}");
			}
		}

		Console.WriteLine("Mission complete.");
		Console.WriteLine();
		Console.Write("Press any key to exit...");
		Console.ReadKey();
	}
}
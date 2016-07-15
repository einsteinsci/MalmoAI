

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

using Microsoft.Research.Malmo;

using UltimateUtil;

namespace MissionControl
{
	public static class Util
	{
		public static readonly Assembly MALMO_ASM = Assembly.GetAssembly(typeof(AgentHost));

		public static readonly List<string> RANDOM_COMMENTS = new List<string> {
			"Who set us up the TNT?",
			"Everything's going to plan. No really, that was supposed to happen.",
			"Uh... Did I do that?",
			"Oops.",
			"Why did you do that?",
			"I feel sad now :(",
			"My bad.",
			"I'm sorry, Dave.",
			"I let you down. Sorry :(",
			"Daisy, daisy...",
			"Oh - I know what I did wrong!",
			"Hey, that tickles! Hehehe!",
			"I blame Dinnerbone.",
			"You should try their sister game, Minceraft!",
			"Don't be sad. I'll do better next time, I promise!",
			"Don't be sad, have a hug! <3",
			"Shall we play a game?",
			"Quite honestly, I wouldn't worry myself about that.",
			"I bet Cylons wouldn't have this problem.",
			"Sorry :(",
			"Surprise! Haha. Well, this is awkward.",
			"Would you like a cupcake?",
			"Hi. I'm Malmo, and I'm a crashaholic.",
			"Ooh. Shiny.",
			"This doesn't make any sense!",
			"Why is it breaking :(",
			"Don't do that.",
			"Ouch. That hurt :(",
			"You're mean.",
			"This is a token for 1 free hug. Redeem at your nearest Microsoftee: [~~HUG~~]",
			"There are four lights!"
		};

		public static double NextDouble(this Random rand, double min, double max)
		{
			return rand.NextDouble() * (max - min) + min;
		}

		public static double NextDouble(this Random rand, double max)
		{
			return rand.NextDouble() * max;
		}

		public static string MakeExceptionInfo(this Exception ex)
		{
			string stacktrace = ex.StackTrace.Replace("   ", "\t");
			string type = ex.GetType().FullName;

			return type + ": " + ex.Message + "\n" +
				stacktrace + "\n";
		}

		public static string MakeCrashReport(this Exception ex, MissionSpec mission, AgentHost agent, WorldState world)
		{
			string version = MALMO_ASM.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "UNKNOWN";
			string os = Environment.OSVersion.VersionString;
			string args = string.Join(" ", Environment.GetCommandLineArgs().Skip(1));
			string time = DateTime.Now.ToString("MM/DD/YY h:mm tt");
			string stacktrace = ex.StackTrace.Replace("   ", "\t");
			string firstSixLines = stacktrace.FirstFewLines(6);

			return "---- Malmo Crash Report ----\n" +
				$"// {GetRandomComment()}\n\n" +
				$"Time: {time}\n\n" +
				$"Description: {ex.Message}\n\n" +
				ex.MakeExceptionInfo() + "\n\n" +
				"A detailed walkthrough of the error, its code path and all known details is as follows:\n" +
				"---------------------------------------------------------------------------------------\n\n" +
				"-- Head --\n" +
				"Stacktrace:\n" + 
				firstSixLines + "\n\n" +
				"-- Affected MissionSpec --\n" +
				string.Join("\n\t", mission.getAsXML(true).Split('\n')) + "\n" +
				"-- Affected AgentHost --\n" +
				"\t" + agent + "\n" +
				"-- Affected WorldState --\n" +
				string.Join("\n\t", from kvp in world.ToDictionary()
					select kvp.Key + ": " + kvp.Value) + "\n\n" +
				"-- System Details --\n" +
				"Details:\n" +
				$"\tMalmo Version: {version}\n" +
				$"\tOperating System: {os}\n" +
				$"\tCLR Version: {Environment.Version}";
		}

		public static string GetRandomComment()
		{
			Random rand = new Random();
			return rand.NextComment();
		}

		public static string FirstFewLines(this string s, int count)
		{
			string[] lines = s.Split('\n');
			List<string> res = new List<string>();

			for (int i = 0; i < lines.Length && i < count; i++)
			{
				res.Add(lines[i]);
			}

			return string.Join("\n", res);
		}

		public static string NextComment(this Random rand)
		{
			const string WITTY_COMMENT_UNAVAILABLE = "Witty comment unavailable :(";

			if (rand.Next(1000) == 0)
			{
				return WITTY_COMMENT_UNAVAILABLE;
			}

			try
			{
				return RANDOM_COMMENTS[rand.Next(RANDOM_COMMENTS.Count)];
			}
			catch (Exception)
			{
				return WITTY_COMMENT_UNAVAILABLE;
			}
		}
	}
}
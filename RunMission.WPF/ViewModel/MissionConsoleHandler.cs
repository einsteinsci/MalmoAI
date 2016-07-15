using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using RunMission.WPF.View;

using UltimateUtil;
using UltimateUtil.UserInteraction;

namespace RunMission.WPF.ViewModel
{
	public class MissionConsoleHandler : VersatileHandlerBase
	{
		public ConsoleView View
		{ get; }

		public static string CurrentInput
		{ get; set; }

		public MissionConsoleHandler(ConsoleView view)
		{
			View = view;
		}

		public override void LogPart(string text, ConsoleColor? color)
		{
			View.Dispatcher.Invoke(() => View.AppendText(text, color));
		}

		public override void LogLine(string line, ConsoleColor color)
		{
			View.Dispatcher.Invoke(() => View.AppendLine(line, color));
		}

		public override string GetString(string prompt)
		{
			View.PromptTxt.Dispatcher.Invoke(() => { View.PromptTxt.Text = prompt; });

			CurrentInput = null;
			View.Dispatcher.Invoke(() => { View.RunBtn.IsEnabled = true; });
			LogPart(prompt, ConsoleColor.Blue);

			while (CurrentInput == null)
			{
				Thread.Sleep(50);

				if (View.Worker.CancellationPending)
				{
					return null;
				}
			}

			LogLine(CurrentInput, ConsoleColor.Blue);
			return CurrentInput.Replace('\n', ' ').Trim();
		}

		public override double GetDouble(string prompt)
		{
			string input = GetString(prompt);

			double d;
			if (double.TryParse(input, out d))
			{
				return d;
			}

			return 0;
		}

		public override string GetSelection(string prompt, IDictionary<string, object> options)
		{
			foreach (KeyValuePair<string, object> kvp in options)
			{
				LogLine($"[{kvp.Key}]: {kvp.Value}", ConsoleColor.White);
			}

			string input = GetString(prompt);

			if (input.IsNullOrWhitespace())
			{
				return options.FirstOrDefault().Key;
			}

			return input;
		}

		public override string GetSelectionIgnorable(string prompt, IDictionary<string, object> options)
		{
			foreach (KeyValuePair<string, object> kvp in options)
			{
				LogLine($"[{kvp.Key}]: {kvp.Value}", ConsoleColor.White);
			}

			string input = GetString(prompt);

			return input.IsNullOrWhitespace() ? null : input;
		}
	}
}
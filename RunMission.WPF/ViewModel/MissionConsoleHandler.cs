using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using RunMission.WPF.View;

using UltimateUtil;
using UltimateUtil.Logging;
using UltimateUtil.UserInteraction;

namespace RunMission.WPF.ViewModel
{
	public class MissionConsoleHandler : VersatileHandlerBase, IDisposable
	{
		public const string LOG_PATH = "log.txt";
		public const string PROMPT_STRING = "mission> ";
		
		public ConsoleView View
		{ get; }

		public string CurrentInput
		{ get; set; }

		public StreamWriter Stream
		{ get; private set; }

		public MissionConsoleHandler(ConsoleView view)
		{
			View = view;

			Stream = new StreamWriter(new FileStream(LOG_PATH, FileMode.Create, FileAccess.ReadWrite));
		}

		public override void LogPart(string text, ConsoleColor? color)
		{
			try
			{
				View.Dispatcher.Invoke(() => View.AppendText(text, color));
			}
			catch (TaskCanceledException)
			{ }

			Stream.Write(text);
		}

		public override void LogLine(string line, ConsoleColor color)
		{
			try
			{
				View.Dispatcher.Invoke(() => View.AppendLine(line, color));
			}
			catch (TaskCanceledException)
			{ }

			Stream.WriteLine(line);
		}

		public override string GetString(string prompt)
		{
			View.PromptTxt.Dispatcher.Invoke(() => { View.PromptTxt.Text = prompt; });

			CurrentInput = null;
			View.Dispatcher.Invoke(() => { View.RunBtn.IsEnabled = true; });

			if (prompt != PROMPT_STRING)
			{
				LogPart(prompt, LogLevel.Interface.GetLevelColor());
			}

			while (CurrentInput == null)
			{
				Thread.Sleep(50);

				if (View.Worker.CancellationPending)
				{
					return null;
				}
			}

			VersatileIO.Write(PROMPT_STRING, LogLevel.Interface.GetLevelColor());
			VersatileIO.WriteLine(CurrentInput, LogLevel.Interface.GetLevelColor());
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

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Stream?.Dispose();
			}
		}
	}
}
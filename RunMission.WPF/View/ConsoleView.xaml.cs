using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using RunMission.WPF.ViewModel;

using UltimateUtil;
using UltimateUtil.UserInteraction;

namespace RunMission.WPF.View
{
	/// <summary>
	/// Interaction logic for ConsoleView.xaml
	/// </summary>
	// ReSharper disable once RedundantExtendsListEntry
	public partial class ConsoleView : UserControl
	{
		public MissionConsoleHandler Handler
		{ get; }

		public bool CommandShutdown
		{ get; private set; }

		public BackgroundWorker Worker
		{ get; }

		public MainWindow Window
		{ get; internal set; }

		public CommandHandler Commands
		{ get; private set; }

		public ConsoleView()
		{
			InitializeComponent();

			Handler = new MissionConsoleHandler(this);

			Worker = new BackgroundWorker {
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			Worker.DoWork += Worker_OnDoWork;
			Worker.RunWorkerCompleted += Worker_OnRunWorkerCompleted;
		}

		public void AppendLine(string line, ConsoleColor color)
		{
			Paragraph p = OutputBox.Document.Blocks.LastOrDefault() as Paragraph;
			if (p == null)
			{
				p = new Paragraph { Margin = new Thickness(0) };
				OutputBox.Document.Blocks.Add(p);
			}

			Run r = new Run(line) { Foreground = color.ToBrush() };
			p.Inlines.Add(r);

			p = new Paragraph { Margin = new Thickness(0) };
			OutputBox.Document.Blocks.Add(p);

			OutputBox.ScrollToEnd();
		}

		public void AppendText(string text, ConsoleColor? color)
		{
			Paragraph p = OutputBox.Document.Blocks.LastOrDefault() as Paragraph;
			if (p == null)
			{
				p = new Paragraph { Margin = new Thickness(0) };
				OutputBox.Document.Blocks.Add(p);
			}

			Run last = p.Inlines.LastOrDefault() as Run;
			Run r = new Run(text);
			if (color == null)
			{
				r.Foreground = last != null ? last.Foreground : new SolidColorBrush(Colors.White);
			}
			else
			{
				r.Foreground = color.Value.ToBrush();
			}
			p.Inlines.Add(r);

			OutputBox.ScrollToEnd();
		}

		private void Worker_OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Window.Close();
		}

		private void Worker_OnDoWork(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				string input = VersatileIO.GetString("mission> ");
				if (input != null)
				{
					if (input.EqualsIgnoreCase("exit"))
					{
						Dispatcher.Invoke(() => { Window.Close(); });
					}

					// Do stuff with command
					Commands.RunCommand(input);
					VersatileIO.WriteLine();
				}

				if (Worker.CancellationPending)
				{
					VersatileIO.Info("Command complete. Exiting.");
					CommandShutdown = true;
					return;
				}
			}
		}

		private void ConsoleView_OnLoaded(object sender, RoutedEventArgs e)
		{
			Commands = new CommandHandler(Window.Agent);
			VersatileIO.SetHandler(Handler);

			Worker.RunWorkerAsync();
			InputBox.Focus();
		}

		private void RunBtn_OnClick(object sender, RoutedEventArgs e)
		{
			MissionConsoleHandler.CurrentInput = InputBox.Text;

			InputBox.Text = "";
			RunBtn.IsEnabled = false;
			InputBox.Focus();
		}

		public void Window_OnClosing(object sender, CancelEventArgs e)
		{
			if (CommandShutdown)
			{
				return;
			}

			Handler.LogLine("Closing as soon as command is completed.", ConsoleColor.Yellow);
			Worker.CancelAsync();
			e.Cancel = true;
		}
	}
}

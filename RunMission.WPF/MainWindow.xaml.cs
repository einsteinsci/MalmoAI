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

using MahApps.Metro.Controls;

using UltimateUtil.Logging;
using UltimateUtil.UserInteraction;

// ReSharper disable once RedundantExtendsListEntry
namespace RunMission.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs _e)
		{
			Console.Window = this;
			Agent.Window = this;

			Logger.Initialize(doTimeStamps: false, minLogLevel: LogLevel.Verbose);
			Logger.Logging += (s, e) => VersatileIO.WriteLine(e.Message, e.Level.GetLevelColor());
			Logger.LoggingPart += (s, e) => VersatileIO.Write(e.Message, e.Level.GetLevelColor());
		}

		private void MainWindow_OnClosing(object sender, CancelEventArgs e)
		{
			Console.Window_OnClosing(sender, e);
		}

		private void MainWindow_OnClosed(object sender, EventArgs e)
		{
			Agent.Window_OnClose(sender, e);
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace RunMission.WPF.View
{
	/// <summary>
	/// Interaction logic for AgentView.xaml
	/// </summary>
	// ReSharper disable once RedundantExtendsListEntry
	public partial class AgentView : UserControl
	{
		public BackgroundWorker Worker
		{ get; }

		public MalmoRunner Runner
		{ get; }

		public MainWindow Window
		{ get; set; }

		public bool AdHoc
		{ get; set; }

		public AgentView()
		{
			InitializeComponent();

			Runner = new MalmoRunner(this);

			Worker = new BackgroundWorker();
			Worker.DoWork += Worker_OnDoWork;
		}

		public void StartRunner()
		{
			Worker.RunWorkerAsync();

			Window.Dispatcher.Invoke(() => {
				WPFUtil.BringExternalWindowToFront("Minecraft 1.8");
				Thread.Sleep(200);
				WPFUtil.BringExternalWindowToFront(Window.Title);

				Window.Console.InputBox.Focus();
			});
		}

		public void StopRunner()
		{
			Runner.Cancel();

			Window.Dispatcher.Invoke(() => Window.Console.InputBox.Focus());
		}

		private void Worker_OnDoWork(object sender, DoWorkEventArgs e)
		{
			Runner.Run(AdHoc);
		}

		private void RunAgentBtn_OnClick(object sender, RoutedEventArgs e)
		{
			StartRunner();
		}

		private void StopAgentBtn_OnClick(object sender, RoutedEventArgs e)
		{
			StopRunner();
		}

		public void Window_OnClose(object sender, EventArgs e)
		{
			StopRunner();
		}
	}
}

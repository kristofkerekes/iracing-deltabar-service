using System;
using System.Windows;

namespace ServiceRunnerApp {
	public partial class App : Application {
		private ServiceRunnerLib.iRacingListener _listener = new ServiceRunnerLib.iRacingListener(iRacingSimulator.Sim.Instance);
		private MainWindow _window = new MainWindow();
		private bool _isActive = false;

		private bool IsActive {
			get { return _isActive; }
			set {
				_isActive = value;
				_window.Data.Activity = value;
			}
		}

		public App() {
			_window.ActivityToggled += ActivityToggled;
			SetServiceState(true);
		}

		~App() {
			SetServiceState(false);
		}

		private void AppStarted(object sender, StartupEventArgs e) {
			_window.Show();
		}

		private void ActivityToggled(object sender, EventArgs e) {
			SetServiceState(!IsActive);
		}

		private void SetServiceState(bool newState) {
			if (newState) {
				iRacingSimulator.Sim.Instance.Start();
			} else {
				iRacingSimulator.Sim.Instance.Stop();
			}
			IsActive = iRacingSimulator.Sim.Instance.Sdk.IsRunning;
		}
	}
}

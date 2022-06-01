using System;
using System.Windows;

namespace ServiceRunnerApp {
	public partial class App : Application {
		private ServiceRunnerLib.iRacingListener _listener = new ServiceRunnerLib.iRacingListener(iRacingSimulator.Sim.Instance);
		private MainWindow _window;
		private SystemTrayHandler _trayhandler = new SystemTrayHandler();

		private bool IsActive {
			get { return _listener.IsActive; }
			set {
				_listener.IsActive = value;
				_window.Data.Activity = _listener.IsActive;
			}
		}

		private string iRacingDocumentsFolder {
			get { return _listener.iRacingDeltaRootFolder; }
			set {
				_listener.iRacingDeltaRootFolder = value;
				_window.Data.iRacingDocumentsFolder = _listener.iRacingDeltaRootFolder;
			}
		}

		private string DeltaFolder {
			get { return _listener.DeltaStorageRootFolder; }
			set {
				_listener.DeltaStorageRootFolder = value;
				_window.Data.DeltaFolder = _listener.DeltaStorageRootFolder;
			}
		}

		private bool ClearExistingLaps {
			get { return _listener.ClearExistingLaps; }
			set {
				_listener.ClearExistingLaps = value;
				_window.Data.ClearExistingLaps = _listener.ClearExistingLaps;
			}
		}

		private bool RunOnSystemStartup {
			get { return RunOnStartup.Startup.IsInStartup(); }
			set {
				if (value) {
					RunOnStartup.Startup.RunOnStartup();
				} else {
					RunOnStartup.Startup.RemoveFromStartup();
				}
				_window.Data.RunOnSystemStartup = RunOnStartup.Startup.IsInStartup();
			}
		}

		public App() {
		}

		~App() {
		}

		private void AppStarted(object sender, StartupEventArgs e) {
			OpenAppCommand.Instance = (OpenAppCommand) FindResource("OpenAppCommand");
			QuitCommand.Instance = (QuitCommand) FindResource("QuitCommand");

			_trayhandler.Initialize((Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)FindResource("NotifyIcon"));
			_trayhandler.OpenAppInvoked += (s, a) => InitializeWindow();
			_trayhandler.QuitInvoked += (s, a) => Shutdown();

		}

		private void InitializeWindow() {
			_window = new MainWindow();
			_window.Data.Activity = IsActive;
			_window.Data.iRacingDocumentsFolder = iRacingDocumentsFolder;
			_window.Data.DeltaFolder = DeltaFolder;
			_window.Data.ClearExistingLaps = ClearExistingLaps;
			_window.Data.RunOnSystemStartup = RunOnSystemStartup;

			_window.Closing += WindowClosing;
			_window.ActivityToggled += ActivityToggled;
			_window.iRacingFolderChanged += iRacingFolderChanged;
			_window.DeltaFolderChanged += DeltaFolderChanged;
			_window.ClearExistingLapsChanged += ClearExistingLapsChanged;
			_window.RunOnSystemStartupChanged += RunOnSystemStartupChanged;

			_window.Show();
		}

		private void WindowClosing(object sender, EventArgs e) {
			_window.Closing -= WindowClosing;
			_window.ActivityToggled -= ActivityToggled;
			_window.iRacingFolderChanged -= iRacingFolderChanged;
			_window.DeltaFolderChanged -= DeltaFolderChanged;
			_window.ClearExistingLapsChanged -= ClearExistingLapsChanged;
			_window.RunOnSystemStartupChanged -= RunOnSystemStartupChanged;

			_trayhandler.AppMinimized();
		}

		private void ActivityToggled(object sender, EventArgs e) {
			IsActive = !IsActive;
		}
		
		private void iRacingFolderChanged(object sender, string e) {
			iRacingDocumentsFolder = e;
		}

		private void DeltaFolderChanged(object sender, string e) {
			DeltaFolder = e;
		}
		private void ClearExistingLapsChanged(object sender, bool e) {
			ClearExistingLaps = e;
		}

		private void RunOnSystemStartupChanged(object sender, bool e) {
			RunOnSystemStartup = e;
		}
	}
}

using System;
using System.Windows;

namespace ServiceRunnerApp {
	public partial class App : Application {
		private ServiceRunnerLib.iRacingListener _listener = new ServiceRunnerLib.iRacingListener(iRacingSimulator.Sim.Instance);
		private MainWindow _window = new MainWindow();

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

		public App() {
		}

		~App() {
		}

		private void AppStarted(object sender, StartupEventArgs e) {
			_window.Data.Activity = IsActive;
			_window.Data.iRacingDocumentsFolder = iRacingDocumentsFolder;
			_window.Data.DeltaFolder = DeltaFolder;
			_window.Data.ClearExistingLaps = ClearExistingLaps;

			_window.ActivityToggled += ActivityToggled;
			_window.iRacingFolderChanged += iRacingFolderChanged;
			_window.DeltaFolderChanged += DeltaFolderChanged;
			_window.ClearExistingLapsChanged += ClearExistingLapsChanged;

			_window.Show();
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
	}
}

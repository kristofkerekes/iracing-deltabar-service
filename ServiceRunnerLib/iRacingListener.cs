using System;
using System.Diagnostics;
using System.IO;

namespace ServiceRunnerLib {
	public class iRacingListener {
		private iRacingSimulator.Sim _iRacingInstance;
		private iRacingSimulator.Drivers.Driver _driver;
		private iRacingSdkWrapper.SessionInfo _sessionInfo;
		private AppSettings _appSettings = new AppSettings();

		public string DeltaStorageRootFolder {
			get {
				return _appSettings.FileSystemSettings.DeltaStorageRootFolder.FullName;
			}
			set {
				_appSettings.DeltaStorageRootFolder = value;
			}
		}

		public string iRacingDeltaRootFolder {
			get {
				return _appSettings.FileSystemSettings.iRacingDeltaRootFolder.FullName;
			}
			set {
				_appSettings.iRacingDeltaRootFolder = value;
			}
		}

		public bool ClearExistingLaps {
			get {
				return _appSettings.ClearExistingLaps;
			}
			set {
				_appSettings.ClearExistingLaps = value;
			}
		}

		public bool IsActive {
			get {
				return _appSettings.IsActive;
			}
			set {
				if (value) {
					iRacingSimulator.Sim.Instance.Start();
				} else {
					iRacingSimulator.Sim.Instance.Stop();
				}
				_appSettings.IsActive = iRacingSimulator.Sim.Instance.Sdk.IsRunning;
			}
		}

		private DeltaLapController DeltaLapController {
			get {
				return new DeltaLapController(ClearExistingLaps, new DirectoryInfo(DeltaStorageRootFolder), new DirectoryInfo(iRacingDeltaRootFolder));
			}
		}

		public iRacingListener(iRacingSimulator.Sim sim) {
			_iRacingInstance = sim;

			_iRacingInstance.Connected += SimConnected;
			_iRacingInstance.Disconnected += SimDisconnected;
		}

		private void SimConnected(object sender, EventArgs e) {
			_iRacingInstance.SessionInfoUpdated += SessionInfoReceived;
		}

		private void SimDisconnected(object sender, EventArgs e) {
			bool success = DeltaLapController.ExportLapsFromSimulator(_driver, _sessionInfo);
			Debug.Assert(success, "Failed to Export data from iRacing!");

			_driver = null;
			_sessionInfo = null;
		}

		private void SessionInfoReceived(object sender, iRacingSdkWrapper.SdkWrapper.SessionInfoUpdatedEventArgs e) {
			_iRacingInstance.SessionInfoUpdated -= SessionInfoReceived;

			_driver = _iRacingInstance.Driver;
			_sessionInfo = _iRacingInstance.SessionInfo;

			bool success = DeltaLapController.ImportLapsToSimulator(_driver, _sessionInfo);
			Debug.Assert(success, "Failed to Import data to iRacing!");
		}
	}
}

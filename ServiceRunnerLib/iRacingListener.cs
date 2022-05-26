using System;

namespace ServiceRunnerLib {
	public class iRacingListener {
		private iRacingSimulator.Sim _iRacingInstance;
		private iRacingSimulator.Drivers.Driver _driver;
		private iRacingSdkWrapper.SessionInfo _sessionInfo;

		private DeltaLapController _deltaLapController = new DeltaLapController(FileSystemSettings.DeltaStorageRootFolder, FileSystemSettings.iRacingDeltaRootFolder);

		public iRacingListener(iRacingSimulator.Sim sim) {
			_iRacingInstance = sim;
			_iRacingInstance.Connected += SimConnected;
			_iRacingInstance.Disconnected += SimDisconnected;
		}

		private void SimConnected(object sender, EventArgs e) {
			_iRacingInstance.SessionInfoUpdated += SessionInfoReceived;
		}

		private void SimDisconnected(object sender, EventArgs e) {
			_driver = null;
			_sessionInfo = null;
		}

		private void SessionInfoReceived(object sender, iRacingSdkWrapper.SdkWrapper.SessionInfoUpdatedEventArgs e) {
			_iRacingInstance.SessionInfoUpdated -= SessionInfoReceived;

			_driver = _iRacingInstance.Driver;
			_sessionInfo = _iRacingInstance.SessionInfo;

			bool success = _deltaLapController.ExportLapsFromSimulator(_driver, _sessionInfo);
		}
	}
}

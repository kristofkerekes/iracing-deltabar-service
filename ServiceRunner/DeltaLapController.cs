using System;
using System.Diagnostics;
using System.IO;

namespace ServiceRunner {
	public class DeltaLapController {
		private DeltaLapStorage _deltaStorage;
		private iRacingDeltaLapStorage _iRacingStorage;

		public DeltaLapController(DirectoryInfo deltaStorageInfo, DirectoryInfo iRacingStorageInfo) {
			_deltaStorage = new DeltaLapStorage(deltaStorageInfo);
			_iRacingStorage = new iRacingDeltaLapStorage(iRacingStorageInfo);
		}

		void ExportLapsFromSimulator(DriverCarInfo) {
			_iRacingStorage.Find();
		}
	}
}

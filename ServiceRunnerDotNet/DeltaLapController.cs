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

		bool ExportLapsFromSimulator(iRacingSimulator.Sim iRacingInstance) {
			string trackId = iRacingInstance.SessionData.Track.CodeName;
			string carId = iRacingInstance.Driver.Car.CarPath;
			DeltaLaps? laps = _iRacingStorage.Find(trackId, carId);
			if (laps == null) {
				return false;
			}

			string seriesId = iRacingInstance.SessionInfo["WeekendInfo"]["SeriesID"].GetValue();
			uint raceWeek = uint.Parse(iRacingInstance.SessionInfo["WeekendInfo"]["RaceWeek"].GetValue());
			return _deltaStorage.Insert(seriesId, raceWeek, (DeltaLaps)laps);
		}
	}
}

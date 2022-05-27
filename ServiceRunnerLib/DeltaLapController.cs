using System.Diagnostics;
using System.IO;

namespace ServiceRunnerLib {
	internal class DeltaLapController {
		private DeltaLapStorage _deltaStorage;
		private iRacingDeltaLapStorage _iRacingStorage;

		public bool ClearExistingLaps { get; set; } = false;

		public DeltaLapController(DirectoryInfo deltaStorageInfo, DirectoryInfo iRacingStorageInfo) {
			_deltaStorage = new DeltaLapStorage(deltaStorageInfo);
			_iRacingStorage = new iRacingDeltaLapStorage(iRacingStorageInfo);
		}

		public bool ExportLapsFromSimulator(iRacingSimulator.Drivers.Driver driver, iRacingSdkWrapper.SessionInfo sessionInfo) {
			string trackId = sessionInfo["WeekendInfo"]["TrackName"].GetValue();
			string carId = driver.Car.CarPath;
			DeltaLaps? laps = _iRacingStorage.Find(trackId, carId);
			if (laps == null) {
				return false;
			}

			string seriesId = sessionInfo["WeekendInfo"]["SeriesID"].GetValue();
			uint raceWeek = uint.Parse(sessionInfo["WeekendInfo"]["RaceWeek"].GetValue());
			if (seriesId.Equals("0") || raceWeek == 0) {
				// Callback to guess / choose series
				Debug.Assert(sessionInfo["WeekendInfo"]["EventType"].GetValue().Equals("Test"));
				return false;
			}

			return _deltaStorage.Insert(seriesId, raceWeek, carId, (DeltaLaps)laps);
		}

		public bool ImportLapsToSimulator(iRacingSimulator.Drivers.Driver driver, iRacingSdkWrapper.SessionInfo sessionInfo) {
			string trackId = sessionInfo["WeekendInfo"]["TrackName"].GetValue();
			string carId = driver.Car.CarPath;
			uint raceWeek = uint.Parse(sessionInfo["WeekendInfo"]["RaceWeek"].GetValue());
			string seriesId = sessionInfo["WeekendInfo"]["SeriesID"].GetValue();
			if (seriesId.Equals("0") || raceWeek == 0) {
				// Callback to guess / choose series
				Debug.Assert(sessionInfo["WeekendInfo"]["EventType"].GetValue().Equals("Test"));
				return false;
			}

			DeltaLaps? laps = _deltaStorage.Find(seriesId, raceWeek, carId);
			if (laps == null) {
				if (ClearExistingLaps) {
					DeltaLaps? existingLaps = _iRacingStorage.Find(trackId, carId);
					existingLaps?.OptimalLap.Delete();
					existingLaps?.BestLap.Delete();
				}
				return false;
			}

			return _iRacingStorage.Insert(trackId, (DeltaLaps)laps);
		}
	}
}

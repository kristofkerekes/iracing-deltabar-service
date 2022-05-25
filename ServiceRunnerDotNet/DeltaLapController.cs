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

		public bool ExportLapsFromSimulator(iRacingSimulator.Sim iRacingInstance) {
			string trackId = iRacingInstance.SessionData.Track.CodeName;
			string carId = iRacingInstance.Driver.Car.CarPath;
			DeltaLaps? laps = _iRacingStorage.Find(trackId, carId);
			if (laps == null) {
				return false;
			}

			string seriesId = iRacingInstance.SessionInfo["WeekendInfo"]["SeriesID"].GetValue();
			uint raceWeek = uint.Parse(iRacingInstance.SessionInfo["WeekendInfo"]["RaceWeek"].GetValue());
			return _deltaStorage.Insert(seriesId, raceWeek, carId, (DeltaLaps)laps);
		}

		public bool ImportLapsToSimulator(iRacingSimulator.Sim iRacingInstance) {
			string carId = iRacingInstance.Driver.Car.CarPath;
			uint raceWeek = uint.Parse(iRacingInstance.SessionInfo["WeekendInfo"]["RaceWeek"].GetValue());
			string seriesId = iRacingInstance.SessionInfo["WeekendInfo"]["SeriesID"].GetValue();
			DeltaLaps? laps = _deltaStorage.Find(seriesId, raceWeek, carId);
			if (laps == null) {
				return false;
			}

			string trackId = iRacingInstance.SessionData.Track.CodeName;
			return _iRacingStorage.Insert(trackId, (DeltaLaps)laps);
		}
	}
}

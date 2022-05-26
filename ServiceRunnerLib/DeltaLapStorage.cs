using System;
using System.Diagnostics;
using System.IO;

namespace ServiceRunnerLib {
	internal class DeltaLapStorage {
        private readonly DirectoryInfo _rootFolder;

		public DeltaLapStorage(DirectoryInfo rootFolder) {
			_rootFolder = rootFolder;
			if (!_rootFolder.Exists) {
				_rootFolder.Create();
			}
		}

		public bool Insert(string seriesId, uint week, string carId, DeltaLaps laps) {
			if (!laps.IsValid()) {
				return false;
			}

			DirectoryInfo[] matchingSeriesDirs = _rootFolder.GetDirectories(seriesId);
			Debug.Assert(matchingSeriesDirs.Length <= 1, "Series ID storage folders should be unique!");

			DirectoryInfo seriesDir = matchingSeriesDirs.Length != 0 ? matchingSeriesDirs[0] : new DirectoryInfo(Path.Combine(_rootFolder.FullName, seriesId));
			if (!seriesDir.Exists) {
				seriesDir.Create();
			}

			string weekFolderName = "W" + week.ToString();
			DirectoryInfo[] matchingWeekDirs = seriesDir.GetDirectories(weekFolderName);
			Debug.Assert(matchingWeekDirs.Length <= 1, "Week storage folders should be unique!");

			DirectoryInfo weekDir = matchingWeekDirs.Length != 0 ? matchingWeekDirs[0] : new DirectoryInfo(Path.Combine(seriesDir.FullName, weekFolderName));
			if (!weekDir.Exists) {
				weekDir.Create();
			}

			DirectoryInfo[] matchingCarDirs = weekDir.GetDirectories(carId);
			Debug.Assert(matchingCarDirs.Length <= 1, "Week storage folders should be unique!");
			DirectoryInfo carDir = matchingCarDirs.Length != 0 ? matchingCarDirs[0] : new DirectoryInfo(Path.Combine(weekDir.FullName, carId));
			if (!carDir.Exists) {
				carDir.Create();
			}

			string weekOptimalLapFile = Path.Combine(carDir.FullName, laps.OptimalLap.Name);
			string weeBestLapFile = Path.Combine(carDir.FullName, laps.BestLap.Name);

			laps.OptimalLap.CopyTo(weekOptimalLapFile);
			laps.BestLap.CopyTo(weeBestLapFile);

			return true;
		}

		public DeltaLaps? Find(string seriesId, uint week, string carId) {
			DirectoryInfo[] matchingSeriesDirs = _rootFolder.GetDirectories(seriesId);
			Debug.Assert(matchingSeriesDirs.Length <= 1, "Series ID storage folders should be unique!");
			if (matchingSeriesDirs.Length == 0) {
				return null;
			}

			DirectoryInfo seriesDir = matchingSeriesDirs[0];

			string weekFolderName = "W" + week.ToString();
			DirectoryInfo[] matchingWeekDirs = seriesDir.GetDirectories(weekFolderName);
			Debug.Assert(matchingWeekDirs.Length <= 1, "Week storage folders should be unique!");
			if (matchingWeekDirs.Length == 0) {
				return null;
			}

			DirectoryInfo weekDir = matchingWeekDirs[0];

			DirectoryInfo[] matchingCarDirs = weekDir.GetDirectories(carId);
			Debug.Assert(matchingCarDirs.Length <= 1, "Week storage folders should be unique!");
			if (matchingCarDirs.Length == 0) {
				return null;
			}

			DirectoryInfo carDir = matchingCarDirs[0];

			FileInfo[] optimalLap = carDir.GetFiles(".olap");
			FileInfo[] bestLap = carDir.GetFiles(".blap");

			Debug.Assert(optimalLap.Length == bestLap.Length, "Best lap and Optimal lap files shouild be stored together!");
			if (optimalLap.Length == 0 || bestLap.Length == 0) {
				return null;
			}

			return new DeltaLaps { OptimalLap = optimalLap[0], BestLap = bestLap[0] };
		}
	}
}

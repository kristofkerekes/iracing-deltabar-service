using System;
using System.Diagnostics;
using System.IO;

namespace ServiceRunner {
	public class DeltaLapStorage {
        private readonly DirectoryInfo _rootFolder;

		public DeltaLapStorage(DirectoryInfo rootFolder) {
			_rootFolder = rootFolder;
		}

		public bool Insert(string seriesId, uint week, DeltaLaps laps) {
			if (!laps.IsValid()) {
				return false;
			}

			DirectoryInfo[] matchingSeriesDirs = _rootFolder.GetDirectories(seriesId);
			Debug.Assert(matchingSeriesDirs.Length <= 1, "Series ID storage folders should be unique!");

			DirectoryInfo seriesDir = matchingSeriesDirs.Length != 0 ? matchingSeriesDirs[0] : new DirectoryInfo(Path.Combine(_rootFolder.FullName, seriesId));

			string weekFolderNamer = "W" + week.ToString();
			DirectoryInfo[] weekDir = seriesDir.GetDirectories(weekFolderNamer);
			Debug.Assert(weekDir.Length <= 1, "Week storage folders should be unique!");

			string weekOptimalLapFile = Path.Combine(weekDir[0].FullName, laps.OptimalLap.Name);
			string weeBestLapFile = Path.Combine(weekDir[0].FullName, laps.BestLap.Name);

			laps.OptimalLap.CopyTo(weekOptimalLapFile);
			laps.BestLap.CopyTo(weeBestLapFile);

			return true;
		}

		public DeltaLaps? Find(string seriesId, uint week, string car) {
			DirectoryInfo[] matchingSeriesDirs = _rootFolder.GetDirectories(seriesId);
			Debug.Assert(matchingSeriesDirs.Length <= 1, "Series ID storage folders should be unique!");
			if (matchingSeriesDirs.Length == 0) {
				return null;
			}

			DirectoryInfo seriesDir = matchingSeriesDirs[0];
			string carMatcher = "_*" + car;
			FileInfo[] optimalLap = seriesDir.GetFiles(carMatcher + ".olap");
			FileInfo[] bestLap = seriesDir.GetFiles(carMatcher + ".blap");

			Debug.Assert(optimalLap.Length == bestLap.Length, "Best lap and Optimal lap files shouild be stored together!");
			if (optimalLap.Length == 0 || bestLap.Length == 0) {
				return null;
			}

			return new DeltaLaps { OptimalLap = optimalLap[0], BestLap = bestLap[0] };
		}
	}
}

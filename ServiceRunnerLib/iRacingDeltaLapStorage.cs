using System;
using System.Diagnostics;
using System.IO;

namespace ServiceRunnerLib {
	internal class iRacingDeltaLapStorage {
		private readonly DirectoryInfo _rootFolder;

		public iRacingDeltaLapStorage(DirectoryInfo rootFolder) {
			_rootFolder = rootFolder;
			Debug.Assert(rootFolder.Exists, "iRacing laps folder should already exist!");
		}

		public bool Insert(string trackId, DeltaLaps laps) {
			if (!laps.IsValid()) {
				return false;
			}

			DirectoryInfo[] matchingTrackDirs = _rootFolder.GetDirectories(trackId);
			Debug.Assert(matchingTrackDirs.Length <= 1, "Track ID storage folders should be unique!");

			DirectoryInfo trackDir = matchingTrackDirs.Length != 0 ? matchingTrackDirs[0] : new DirectoryInfo(Path.Combine(_rootFolder.FullName, trackId));
			if (!trackDir.Exists) {
				trackDir.Create();
			}

			string optimalLapFile = Path.Combine(trackDir.FullName, laps.OptimalLap.Name);
			string bestLapFile = Path.Combine(trackDir.FullName, laps.BestLap.Name);

			laps.OptimalLap.CopyTo(optimalLapFile, true);
			laps.BestLap.CopyTo(bestLapFile, true);

			return true;
		}

		public DeltaLaps? Find(string trackId, string car) {
			DirectoryInfo[] matchingTrackDirs = _rootFolder.GetDirectories(trackId);
			Debug.Assert(matchingTrackDirs.Length <= 1, "Track ID storage folders should be unique!");
			if (matchingTrackDirs.Length == 0) {
				return null;
			}

			DirectoryInfo trackDir = matchingTrackDirs[0];
			string carMatcher = "*_" + car;
			FileInfo[] optimalLap = trackDir.GetFiles(carMatcher + ".olap");
			FileInfo[] bestLap = trackDir.GetFiles(carMatcher + ".blap");

			Debug.Assert(optimalLap.Length == bestLap.Length, "Best lap and Optimal lap files shouild be avaiable simultaneously!");
			if (optimalLap.Length == 0 || bestLap.Length == 0) {
				return null;
			}

			return new DeltaLaps { OptimalLap = optimalLap[0], BestLap = bestLap[0] };
		}
	}
}

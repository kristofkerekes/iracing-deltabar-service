using System.IO;

namespace ServiceRunnerLib {
	internal struct DeltaLaps {
		public FileInfo OptimalLap { get; set; }
		public FileInfo BestLap { get; set; }

		public bool IsValid() => OptimalLap.Exists && BestLap.Exists;
	}
}

using System;
using System.IO;

namespace ServiceRunnerLib {
	internal class FileSystemSettings {
		private static string DefaultiRacingLaptimesFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "iRacing\\lapfiles");
		private static string DefaultDeltaStorageFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "iRacingDeltaService");

		public DirectoryInfo iRacingDeltaRootFolder = new DirectoryInfo(DefaultiRacingLaptimesFolder);
		public DirectoryInfo DeltaStorageRootFolder = new DirectoryInfo(DefaultDeltaStorageFolder);

		internal void LoadFolderInfo() {
			// TODO load .ini file with information
		}
		internal void SaveFolderInfo(string newdeltaStorageRootFolder, string newiRacingDeltaRootFolder) {
			// TODO save .ini file with information
			iRacingDeltaRootFolder = new DirectoryInfo(newiRacingDeltaRootFolder);
			DeltaStorageRootFolder = new DirectoryInfo(newdeltaStorageRootFolder);
		}
	}
}

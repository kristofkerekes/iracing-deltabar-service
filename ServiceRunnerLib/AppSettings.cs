using Newtonsoft.Json;
using System.IO;

namespace ServiceRunnerLib {
	struct AppSettingsData {
		public bool ClearExistingLaps { get; set; }
		public bool IsActive { get; set; }
		public string DeltaStorageRootFolder { get; set; }
		public string iRacingDeltaRootFolder { get; set; }
	}

	internal class AppSettings {
		private string AppSettingsFile => Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
		private bool _noSerialization = false;

		private bool _clearExistingLaps = false;
		public bool ClearExistingLaps {
			get {
				return _clearExistingLaps;
			}
			set {
				_clearExistingLaps = value;
				Serialize();
			}
		}

		private bool _isActive = true;
		public bool IsActive {
			get {
				return _isActive;
			}
			set {
				_isActive = value;
				Serialize();
			}
		}

		private FileSystemSettings _fileSystemSettings = new FileSystemSettings();
		public FileSystemSettings FileSystemSettings { get { return _fileSystemSettings; } }
		public string DeltaStorageRootFolder {
			set {
				_fileSystemSettings.DeltaStorageRootFolder = new DirectoryInfo(value);
				Serialize();
			}
		}
		public string iRacingDeltaRootFolder {
			set {
				_fileSystemSettings.iRacingDeltaRootFolder = new DirectoryInfo(value);
				Serialize();
			}
		}

		internal AppSettings() {
			Deserialize();
		}
		~AppSettings() {
			Serialize();
		}

		internal void Serialize() {
			if (_noSerialization) {
				return;
			}

			FileInfo appFileInfo = new FileInfo(AppSettingsFile);
			if (!appFileInfo.Exists) {
				appFileInfo.Directory.Create();
			}

			AppSettingsData serializableData = new AppSettingsData {
				ClearExistingLaps = ClearExistingLaps,
				IsActive = IsActive,
				DeltaStorageRootFolder = FileSystemSettings.DeltaStorageRootFolder.FullName,
				iRacingDeltaRootFolder = FileSystemSettings.iRacingDeltaRootFolder.FullName
			};
			string appSettingsDataSerialized = JsonConvert.SerializeObject(serializableData, Formatting.Indented);
			File.WriteAllText(AppSettingsFile, appSettingsDataSerialized);
		}

		internal void Deserialize() {
			if (!File.Exists(AppSettingsFile)) {
				Serialize();
				return;
			}
			_noSerialization = true;

			string fileContent = File.ReadAllText(AppSettingsFile);
			if (fileContent.Length == 0) {
				return;
			}

			AppSettingsData serializableData = JsonConvert.DeserializeObject<AppSettingsData>(fileContent);
			ClearExistingLaps = serializableData.ClearExistingLaps;
			IsActive = serializableData.IsActive;
			DeltaStorageRootFolder = serializableData.DeltaStorageRootFolder;
			iRacingDeltaRootFolder = serializableData.iRacingDeltaRootFolder;
			_noSerialization = false;
		}
	}
}

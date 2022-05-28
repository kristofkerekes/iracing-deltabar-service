using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace ServiceRunnerApp {
	internal class MainWindowData : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged; 
		private void OnPropertyChanged(string info) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) {
				handler(this, new PropertyChangedEventArgs(info));
			}
		}

		private bool _activity = false;
		public bool Activity { 
			get {
				return _activity;
			}
			set {
				_activity = value;
				OnPropertyChanged("Activity");
				OnPropertyChanged("ActivityColor");
				OnPropertyChanged("ActivityText");
			}
		}
		public Brush ActivityColor { get { return Activity ? Brushes.Green : Brushes.Red; } }
		public string ActivityText { get { return Activity ? "Active" : "Inactive"; } }

		private string _iRacingDocumentsFolder = "";
		public string iRacingDocumentsFolder {
			get {
				return _iRacingDocumentsFolder;
			}
			set {
				_iRacingDocumentsFolder = value;
				OnPropertyChanged("iRacingDocutmentsFolder");
			}
		}

		private string _deltaFolder = "";
		public string DeltaFolder {
			get {
				return _deltaFolder;
			}
			set {
				_deltaFolder = value;
				OnPropertyChanged("DeltaFolder");
			}
		}

		private bool _clearExistingLaps = false;
		public bool ClearExistingLaps {
			get {
				return _clearExistingLaps;
			}
			set {
				_clearExistingLaps = value;
				OnPropertyChanged("ClearExistingLaps");
			}
		}
	}

	public partial class MainWindow : Window {
		internal MainWindowData Data { get; set; }

		public MainWindow() {
			InitializeComponent();
			Data = new MainWindowData();
			DataContext = Data;
		}

		private void InitializeSystemTray() {
		}

		private void ActivityButtonClicked(object sender, RoutedEventArgs e) {
			ActivityToggled?.Invoke(this, e);
		}

		private void BrowseiRacingFolderClicked(object sender, RoutedEventArgs e) {
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();
			if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				iRacingFolderChanged?.Invoke(this, folderDialog.SelectedPath);
			}
		}
		private void BrowseDeltaFolderClicked(object sender, RoutedEventArgs e) {
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();
			if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				DeltaFolderChanged?.Invoke(this, folderDialog.SelectedPath);
			}
		}

		private void ClearExistingLapsToggled(object sender, RoutedEventArgs e) {
			ClearExistingLapsChanged?.Invoke(this, ClearExistingLapsCheck.IsChecked == true);
		}

		public event EventHandler ActivityToggled;
		public event EventHandler<string> iRacingFolderChanged;
		public event EventHandler<string> DeltaFolderChanged;
		public event EventHandler<bool> ClearExistingLapsChanged;
	}
}

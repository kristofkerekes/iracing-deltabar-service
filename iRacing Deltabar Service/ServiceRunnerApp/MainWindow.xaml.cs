using System;
using System.ComponentModel;
using System.Windows;
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
	}

	public partial class MainWindow : Window {
		internal MainWindowData Data { get; set; }

		public MainWindow() {
			InitializeComponent();
			Data = new MainWindowData();
			this.DataContext = Data;
		}

		private void InitializeSystemTray() {
		}

		private void ActivityButtonClicked(object sender, RoutedEventArgs e) {
			ActivityToggled?.Invoke(this, e);
		}

		public event EventHandler ActivityToggled;
	}
}

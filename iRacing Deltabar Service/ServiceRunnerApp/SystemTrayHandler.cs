using Hardcodet.Wpf.TaskbarNotification;
using System;

namespace ServiceRunnerApp {
	public class SystemTrayHandler {
		TaskbarIcon _taskbarIcon;

		public EventHandler QuitInvoked;
		public EventHandler OpenAppInvoked;

		public SystemTrayHandler() {
		}

		public void Initialize(TaskbarIcon resourceIcon) {
			_taskbarIcon = resourceIcon;
			OpenAppCommand.Instance.Invoked += OpenAppClicked;
			QuitCommand.Instance.Invoked += QuitClicked;
		}

		private void QuitClicked(object sender, EventArgs e) {
			QuitInvoked?.Invoke(this, e);
		}

		private void OpenAppClicked(object sender, EventArgs e) {
			OpenAppInvoked?.Invoke(this, e);
		}

		public void AppMinimized() {
			_taskbarIcon.ShowBalloonTip("Still Running...", "iRacing Delta Service will be running in the background.", _taskbarIcon.Icon, true);
		}
	}
}

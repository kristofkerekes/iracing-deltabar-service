using System.Windows;

namespace ServiceRunnerApp {
	public partial class App : Application {
		private ServiceRunnerLib.iRacingListener listener = new ServiceRunnerLib.iRacingListener(iRacingSimulator.Sim.Instance);

		public App() {
			iRacingSimulator.Sim.Instance.Start();
		}

		~App() {
			iRacingSimulator.Sim.Instance.Stop();
		}
	}
}

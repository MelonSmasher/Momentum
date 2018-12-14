using System;
using ConsoleMenu;
using Momentum.Config;
using Momentum.Config.Slack;

namespace Momentum.Services {
	public class ServiceManager {
		private readonly string[] _services = {"Slack", "Cancel"};
		private readonly string[] _yesNo = {"Yes", "No"};

		public ServiceManager() { }

		/// <summary>
		/// Adds a new service to our config
		/// </summary>
		/// <returns></returns>
		public int AddService() {
			// Create a config interface
			var mConfigInterface = new ConfigInterface();

			// Prompt for the new service type
			Console.WriteLine("Choose a service to configure...");
			var service = new Menu().Render(_services);
			switch (service.ToLower()) {
				case "slack":
					// Create a new slack config
					var slackConfig = new SlackModel();
					// Prompt for the config options
					slackConfig.PromptForNew();
					// Add the config to our main config
					//mConfigInterface.Config.SlackModels.Add(slackConfig);
					break;
				case "cancel":
					Console.WriteLine("Oh well maybe next time.");
					return 130;
				default:
					Console.WriteLine("You dirty dog, you!");
					return 2; // No way you should have gotten here!
			}

			// Prompt to confirm
			var shouldSave = new Menu().Render(_yesNo);
			switch (shouldSave.ToLower()) {
				case "yes":
					return mConfigInterface.WriteConfig() ? 0 : 2;
				default:
					// If we did not get yes return a 130
					Console.WriteLine("Oh well maybe next time.");
					return 130;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public int DelService() { return 0; }
	}
}
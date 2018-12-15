using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleMenu;
using Momentum.Menu;
using Momentum.Config.Slack;
using Momentum.File;

namespace Momentum.Services {
	public class ServiceManager {
		private readonly List<MenuChoice> _services = new List<MenuChoice> {
			new MenuChoice("Slack", "slack"),
			new MenuChoice("Cancel", "cancel")
		};

		private readonly List<MenuChoice> _yesNo = new List<MenuChoice> {
			new MenuChoice("Yes", "yes"),
			new MenuChoice("No", "no")
		};

		public ServiceManager() { }

		/// <summary>
		/// Adds a new service to our config
		/// </summary>
		/// <returns></returns>
		public int AddService() {
			// Create a config interface
			var mConfigInterface = new ConfigInterface();
			var serviceMenu = new TypedMenu<MenuChoice>(_services, "Choose a service to configure", x => x.Name);
			var ynMenu = new TypedMenu<MenuChoice>(_yesNo, "Save this configuration", x => x.Name);

			var service = serviceMenu.Display();
			switch (service.Id) {
				case "slack":
					// Create a new slack config
					var slackConfig = new SlackModel();
					// get the slack models or create a new list
					var slackModels = mConfigInterface.Config.SlackModels ?? new List<SlackModel>();
					// Prompt for the config options
					slackConfig.PromptForNew();
					// Add the config to our main config
					slackModels.Add(slackConfig);
					// Set the new array
					mConfigInterface.Config.SlackModels = slackModels;
					break;
				case "cancel":
					Console.WriteLine("Oh well maybe next time.");
					return 130;
				default:
					Console.WriteLine("You dirty dog, you!");
					return 2; // No way you should have gotten here!
			}

			// Prompt to confirm
			var shouldSave = ynMenu.Display();
			switch (shouldSave.Id) {
				case "yes":
					Console.Write("Saving... ");
					if (mConfigInterface.WriteConfig()) {
						Console.WriteLine("Saved!");
						return 0;
					} else {
						Console.WriteLine("Save Error!");
						return 2;
					}
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
		public int DelService() {
			// Create a config interface
			var mConfigInterface = new ConfigInterface();
			var serviceMenu = new TypedMenu<MenuChoice>(_services, "Choose a service type to delete", x => x.Name);
			var ynMenu = new TypedMenu<MenuChoice>(_yesNo, "Save this configuration", x => x.Name);

			var service = serviceMenu.Display();
			switch (service.Id) {
				case "slack":
					// get the slack models or create a new list
					var slackModels = mConfigInterface.Config.SlackModels;
					if (slackModels == null || !slackModels.Any()) {
						Console.WriteLine("No Slack configurations to delete!");
						return 2;
					}

					var slackMenu =
						new TypedMenu<SlackModel>(slackModels, "Choose a slack config delete", x => x.Label);
					var choice = slackMenu.Display();
					slackModels.Remove(choice);
					mConfigInterface.Config.SlackModels = slackModels;
					break;
				case "cancel":
					Console.WriteLine("Oh well maybe next time.");
					return 130;
				default:
					Console.WriteLine("You dirty dog, you!");
					return 2; // No way you should have gotten here!
			}

			// Prompt to confirm
			var shouldSave = ynMenu.Display();
			switch (shouldSave.Id) {
				case "yes":
					Console.Write("Saving... ");
					if (mConfigInterface.WriteConfig()) {
						Console.WriteLine("Saved!");
						return 0;
					} else {
						Console.WriteLine("Save Error!");
						return 2;
					}
				default:
					// If we did not get yes return a 130
					Console.WriteLine("Oh well maybe next time.");
					return 130;
			}
		}
	}
}
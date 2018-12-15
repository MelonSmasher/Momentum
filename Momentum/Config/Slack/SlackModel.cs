using System;

namespace Momentum.Config.Slack {
	public class SlackModel : BaseModel {
		public string WebHook { get; set; }

		public string Channel { get; set; }

		public string BotName { get; set; }

		public string IconUrl { get; set; }

		public SlackModel() { }

		public SlackModel(string label, string webHook, string channel, string botName, string iconUrl) {
			Label = label;
			WebHook = webHook;
			Channel = channel;
			BotName = botName;
			IconUrl = iconUrl;
		}

		public override void PromptForNew() {
			while (string.IsNullOrEmpty(Label)) {
				Console.WriteLine("Enter a label for this Slack configuration: ");
				Label = Console.ReadLine();
				if (string.IsNullOrEmpty(Label)) Console.WriteLine("You must enter a label for this configuration!");
			}

			while (string.IsNullOrEmpty(WebHook)) {
				Console.WriteLine("Enter the Slack web hook URL: ");
				WebHook = Console.ReadLine();
				if (string.IsNullOrEmpty(WebHook)) Console.WriteLine("You must enter a URL for the web hook!");
				if (!CheckUrlValidity(WebHook)) {
					WebHook = null;
					Console.WriteLine("You must enter a valid URL for the web hook!");
				}
			}

			Console.WriteLine("Enter the Slack channel (leave blank to use the web hook channel): ");
			Channel = Console.ReadLine();

			Console.WriteLine("Enter the Slack bot name (leave blank to use \"Momentum Bot\"): ");
			BotName = Console.ReadLine();
			if (string.IsNullOrEmpty(BotName)) BotName = "Momentum Bot";

			while (string.IsNullOrEmpty(IconUrl)) {
				Console.WriteLine("Enter the Slack bot icon url (leave blank to use the Momentum icon): ");
				IconUrl = Console.ReadLine();
				if (string.IsNullOrEmpty(IconUrl)) IconUrl = Resources.LogoUrl;
				if (!CheckUrlValidity(IconUrl)) {
					IconUrl = null;
					Console.WriteLine("You must enter a valid URL for the icon url!");
				}
			}
		}
	}
}
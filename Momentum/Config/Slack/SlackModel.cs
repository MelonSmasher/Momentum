using System;

namespace Momentum.Config.Slack {
	public class SlackModel {
		public string WebHook {
			get { return WebHook; }
			set { WebHook = value; }
		}

		public string Channel {
			get { return Channel; }
			set { Channel = value; }
		}

		public string BotName {
			get { return BotName; }
			set { BotName = value; }
		}

		public string IconUrl {
			get { return IconUrl; }
			set { IconUrl = value; }
		}


		public SlackModel() { }

		public SlackModel(string webHook, string channel, string botName, string iconUrl) {
			WebHook = webHook;
			Channel = channel;
			BotName = botName;
			IconUrl = iconUrl;
		}

		public void PromptForNew() {
			Console.WriteLine("Enter the Slack web hook URL: ");
			WebHook = Console.ReadLine();
			Console.WriteLine("Enter the Slack channel: ");
			Channel = Console.ReadLine();
			Console.WriteLine("Enter the Slack bot name: ");
			BotName = Console.ReadLine();
			Console.WriteLine("Enter the Slack bot icon url: ");
			IconUrl = Console.ReadLine();
		}
	}
}
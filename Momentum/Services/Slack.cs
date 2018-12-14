using System;
using Momentum.Config.Slack;
using Slack.Webhooks;

namespace Momentum.Services {
	public class Slack {

		private SlackModel _slackConfig;
		private SlackClient _client;
		private SlackMessage _message = new SlackMessage();
		
		public Slack(SlackModel slackConfig) {
			// Initialize our slack config and our slack client.
			_slackConfig = slackConfig;
			_client = new SlackClient(slackConfig.WebHook);
			// Build the message
			if (!string.IsNullOrEmpty(_slackConfig.IconUrl)) _message.IconUrl = new Uri(_slackConfig.IconUrl);
			if (!string.IsNullOrEmpty(_slackConfig.Channel)) _message.Channel = _slackConfig.Channel;
			if (!string.IsNullOrEmpty(_slackConfig.BotName)) _message.Username = _slackConfig.BotName;
		}
		
	}
}
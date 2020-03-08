using System;
using System.Collections.Generic;
using System.Net;
using Momentum.Config.Slack;
using Momentum.Veeam;
using Slack.Webhooks;

namespace Momentum.Services {
	public class Slack {
		private SlackMessage _message = new SlackMessage();
		private VeeamSession _veeamSession;

		public Slack(SlackModel slackConfig, VeeamSession veeamSession) {
			// Create a new client
			var client = new SlackClient(slackConfig.WebHook);
			// Set the Veeam Session Data
			_veeamSession = veeamSession;

			// Build the message
			if (!string.IsNullOrEmpty(slackConfig.IconUrl)) _message.IconUrl = new Uri(slackConfig.IconUrl);
			if (!string.IsNullOrEmpty(slackConfig.Channel)) _message.Channel = slackConfig.Channel;
			if (!string.IsNullOrEmpty(slackConfig.BotName)) _message.Username = slackConfig.BotName;

			// Build the rich format with an attachment
			_message.Attachments = new List<SlackAttachment> {FormAttachment()};

			//Enable TLS 1.2
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			// Post the message
			client.Post(_message);
			Console.WriteLine("Slack message sent.");
		}

		private SlackAttachment FormAttachment() {
			var slackAttachment = new SlackAttachment();

			slackAttachment.Title = _veeamSession.Name + " - " + _veeamSession.Result;
			slackAttachment.Pretext = _veeamSession.JobName + " (" +
			                          _veeamSession.JobType + ") " +
			                          " finished with a " + _veeamSession.Result + " result.";
			slackAttachment.Fallback = _veeamSession.JobName + " (" +
			                           _veeamSession.JobType + ") " +
			                           " finished with a " + _veeamSession.Result + " result.";
			slackAttachment.Footer = "Momentum";
			slackAttachment.FooterIcon = Resources.LogoUrl;

			switch (_veeamSession.Result.ToLower()) {
				case "success":
					slackAttachment.Pretext = Resources.Slack.Emoji.Success + " " + slackAttachment.Pretext;
					slackAttachment.Color = Resources.Colors.Success;
					break;
				case "failed":
					slackAttachment.Pretext = Resources.Slack.Emoji.Failed + " " + slackAttachment.Pretext;
					slackAttachment.Color = Resources.Colors.Failed;
					break;
				case "warning":
					slackAttachment.Pretext = Resources.Slack.Emoji.Warning + " " + slackAttachment.Pretext;
					slackAttachment.Color = Resources.Colors.Warning;
					break;
				case "none":
					slackAttachment.Pretext = Resources.Slack.Emoji.None + " " + slackAttachment.Pretext;
					slackAttachment.Color = Resources.Colors.None;
					break;
				default:
					slackAttachment.Pretext = Resources.Slack.Emoji.Default + " " + slackAttachment.Pretext;
					slackAttachment.Color = Resources.Colors.Default;
					break;
			}

			slackAttachment.Fields = new List<SlackField> {
				new SlackField {
					Title = "Duration",
					Value = _veeamSession.Duration
				},
				/*new SlackField {
					Title = "Bottleneck",
					Value = _veeamSession.Bottleneck
				},*/
				new SlackField {
					Title = "Processing Rate",
					Value = _veeamSession.RateHuman
				},
				new SlackField {
					Title = "Data Processed",
					Value = _veeamSession.ProcessedHuman
				},
				new SlackField {
					Title = "Data Read",
					Value = _veeamSession.ReadHuman
				},
				new SlackField {
					Title = "Data Transferred",
					Value = _veeamSession.TransferredHuman
				},
				new SlackField {
					Title = "Successes",
					Value = _veeamSession.SuccessCount.ToString()
				},
				new SlackField {
					Title = "Warnings",
					Value = _veeamSession.WarningCount.ToString()
				},
				new SlackField {
					Title = "Failures",
					Value = _veeamSession.FailureCount.ToString()
				},
			};

			return slackAttachment;
		}
	}
}
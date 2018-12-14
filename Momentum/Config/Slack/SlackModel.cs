using SettingsProviderNet;

namespace Momentum.Config.Slack
{
    public class SlackModel
    {
        [Key("WebHook")]
        public string WebHook
        {
            get { return WebHook; }
            set { WebHook = value; }
        }
        
        [Key("Channel")]
        public string Channel
        {
            get { return Channel; }
            set { Channel = value; }
        }

        [Key("BotName")]
        public string BotName
        {
            get { return BotName; }
            set { BotName = value; }
        }

        [Key("IconUrl")]
        public string IconUrl
        {
            get { return IconUrl; }
            set { IconUrl = value; }
        }


        public SlackModel()
        {
        }

        public SlackModel(string webHook, string channel, string botName, string iconUrl)
        {
            WebHook = webHook;
            Channel = channel;
            BotName = botName;
            IconUrl = iconUrl;
        }
    }
}
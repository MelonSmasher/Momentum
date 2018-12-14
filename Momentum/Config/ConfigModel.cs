using Momentum.Config.Slack;
using System.Collections.Generic;
using SettingsProviderNet;

namespace Momentum.Config
{
    public class ConfigModel
    {
        [Key("SlackModels")]
        public IEnumerable<SlackModel> SlackModels
        {
            get { return SlackModels; }
            set { SlackModels = value; }
        }

        public ConfigModel()
        {
        }

        public ConfigModel(IEnumerable<SlackModel> slackModels)
        {
            SlackModels = slackModels;
        }
    }
}
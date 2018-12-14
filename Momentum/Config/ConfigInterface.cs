using SettingsProviderNet;

namespace Momentum.Config
{
    public class ConfigInterface
    {
        public ConfigInterface()
        {
        }

        public ConfigModel ReadConfig()
        {
            var settingsProvider = new SettingsProvider();
            var configModel = settingsProvider.GetSettings<ConfigModel>();

            return configModel == null ? new ConfigModel() : configModel;
        }

        public bool SaveConfig(ConfigModel config = null)
        {
            var settingsProvider = new SettingsProvider();

            if (config == null) return false;
            settingsProvider.SaveSettings(config);
            return true;
        }
    }
}
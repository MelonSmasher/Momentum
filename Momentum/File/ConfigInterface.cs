using System.IO;
using Momentum.Config;

namespace Momentum.File {
	public class ConfigInterface {
		public ConfigModel Config = new ConfigModel();
		private readonly JsonInterface _jsonInterface = new JsonInterface();

        public ConfigInterface() {
            // Always make sure the config dir exists
            Directory.CreateDirectory(Resources.ConfigDirectory);
            // If the config file does not exist
            if (!System.IO.File.Exists(Resources.ConfigFile)) {
				// Write the empty config
				WriteConfig();
			} else {
				// Read the config
				ReadConfig();
			}
		}

		/// <summary>
		/// Reads the config file from the file system
		/// </summary>
		/// <returns></returns>
		public void ReadConfig() {
			// Read the config from our file and deserialize it into our config class
			Config = _jsonInterface.ReadFromJsonFile<ConfigModel>(Resources.ConfigFile);
		}

		/// <summary>
		/// Writes the config to the file system
		/// </summary>
		/// <returns>bool</returns>
		public bool WriteConfig() {
			_jsonInterface.WriteToJsonFile<ConfigModel>(Resources.ConfigFile, Config);
			return true;
		}
	}
}
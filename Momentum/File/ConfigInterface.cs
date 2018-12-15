using System;
using System.IO;
using Momentum.Config;

namespace Momentum.File {
	public class ConfigInterface {
		private string _configPath;

		public ConfigModel Config = new ConfigModel();
		private readonly JsonInterface _jsonInterface = new JsonInterface();

		public ConfigInterface() {
			// If the config file does not exist
			if (!System.IO.File.Exists(InitConfigFile())) {
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
			Config = _jsonInterface.ReadFromJsonFile<ConfigModel>(_configPath);
		}

		/// <summary>
		/// Writes the config to the file system
		/// </summary>
		/// <returns>bool</returns>
		public bool WriteConfig() {
			_jsonInterface.WriteToJsonFile<ConfigModel>(_configPath, Config);
			return true;
		}

		/// <summary>
		/// Create the configuration file if it does not exist.
		/// Reads the config file if it exists.
		/// </summary>
		/// <returns></returns>
		public string InitConfigFile() {
			// Build the configuration directory path
			var configDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
			                "\\Momentum\\config";
			// Create the directory if it does not exist
			Directory.CreateDirectory(configDir);
			// Build the path to the config file
			var configPath = configDir + "\\momentum.json";
			_configPath = configPath;
			return configPath;
		}
	}
}
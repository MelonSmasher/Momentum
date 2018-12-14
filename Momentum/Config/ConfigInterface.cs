using System;
using System.IO;
using Newtonsoft.Json;

namespace Momentum.Config {
	public class ConfigInterface {
		private string _configPath;

		public ConfigModel Config = new ConfigModel();

		public ConfigInterface() {
			// If the config file does not exist
			if (!File.Exists(InitConfigFile())) {
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
			Config = ReadFromJsonFile<ConfigModel>(_configPath);
		}

		/// <summary>
		/// Writes the config to the file system
		/// </summary>
		/// <returns>bool</returns>
		public bool WriteConfig() {
			WriteToJsonFile<ConfigModel>(_configPath, Config);
			return true;
		}

		private static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new() {
			TextWriter writer = null;
			try {
				var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
				writer = new StreamWriter(filePath, append);
				writer.Write(contentsToWriteToFile);
			} finally {
				if (writer != null)
					writer.Close();
			}
		}

		private static T ReadFromJsonFile<T>(string filePath) where T : new() {
			TextReader reader = null;
			try {
				reader = new StreamReader(filePath);
				var fileContents = reader.ReadToEnd();
				return JsonConvert.DeserializeObject<T>(fileContents);
			} finally {
				if (reader != null)
					reader.Close();
			}
		}

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
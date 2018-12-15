using System;
using System.Collections.Generic;
using System.Linq;
using Momentum.File;
using Momentum.Veeam;

namespace Momentum.Services {
	public class Runner {
		private ConfigInterface _configInterface = new ConfigInterface();
		private JsonInterface _jsonInterface = new JsonInterface();
		private IEnumerable<string> _files;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="files"></param>
		public Runner(IEnumerable<string> files) {
			_files = files;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run() {
			// Gather the session data
			List<VeeamSession> veeamSessions = ReadSessionFiles(_files);
			// Process slack options
			var slackConfigs = _configInterface.Config.SlackModels;
			if (slackConfigs != null && slackConfigs.Any()) {
				foreach (var slackConfig in slackConfigs) {
					foreach (var veeamSession in veeamSessions) {
						var slack = new Slack(slackConfig, veeamSession);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="files"></param>
		/// <returns></returns>
		private List<VeeamSession> ReadSessionFiles(IEnumerable<string> files) {
			List<VeeamSession> veeamSessions = new List<VeeamSession>();
			foreach (var file in files) {
				// Debug output @todo get rid of this line
				Console.WriteLine("Processing Session File: " + file);
				// Add the Veeam sessions from the json files to our array
				veeamSessions.Add(_jsonInterface.ReadFromJsonFile<VeeamSession>(file));
			}
			return veeamSessions;
		}
	}
}
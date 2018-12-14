using System;
using System.Collections.Generic;
using System.Linq;
using Momentum.Config;

namespace Momentum.Services {
	public class Runner {
		
		private ConfigInterface _configInterface = new ConfigInterface();
		private IEnumerable<string> _files;
		
		public Runner(IEnumerable<string> files) {
			_files = files;
		}

		public void Run() {
			foreach (var file in _files) {
				Console.WriteLine("Processing: " + file);
				
				// Process slack options
				var slacks = _configInterface.Config.SlackModels;
				if (slacks != null && slacks.Any()) {
					
				}
			}
		}
		
	}
}
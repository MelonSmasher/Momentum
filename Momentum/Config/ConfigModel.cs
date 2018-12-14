using Momentum.Config.Slack;
using System.Collections.Generic;

namespace Momentum.Config {
	public class ConfigModel {
		public List<SlackModel> SlackModels { get; set; }
		public ConfigModel() { }
	}
}
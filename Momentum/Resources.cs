using System;

namespace Momentum {
	public class Resources {
		public const string LogoUrl = "https://raw.githubusercontent.com/MelonSmasher/Momentum/master/assets/img/Momentum.png";
        public static readonly string ConfigDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                            "\\Momentum\\config";
        public static readonly string ConfigFile = ConfigDirectory + "\\momentum.json";

        public class Colors {
			public const string None = "#c9cfce";
			public const string Default = "#24aef4";
			public const string Success = "#24f46a";
			public const string Warning = "#f4d224";
			public const string Failed = "#f42446";
		}

		public class Slack {
			public class Emoji {
				public const string None = ":grey_question:";
				public const string Default = ":no_mouth:";
				public const string Success = ":green_heart:";
				public const string Warning = ":warning:";
				public const string Failed = ":collision:";
			}
		}
	}
}
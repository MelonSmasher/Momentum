using System;

namespace Momentum.Config {
	public class BaseModel {
		public string Label { get; set; }

		public BaseModel() { }

		public BaseModel(string label) {
			Label = label;
		}

		public virtual void PromptForNew() { }

		public static bool CheckUrlValidity(string source) {
			return Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttps;
		}
	}
}
namespace Momentum.Config {
	public class BaseModel {
		public string Label { get; set; }

		public BaseModel() { }

		public BaseModel(string label) {
			Label = label;
		}

		public virtual void PromptForNew() { }
	}
}
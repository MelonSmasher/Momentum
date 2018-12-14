namespace Momentum.Menu {
	public class MenuChoice {
		public string Name { get; set; }
		public string Id { get; set; }

		public MenuChoice(string name, string id) {
			Name = name;
			Id = id;
		}
	}
}
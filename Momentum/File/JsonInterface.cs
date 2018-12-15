using System.IO;
using Newtonsoft.Json;

namespace Momentum.File {
	public class JsonInterface {
		public JsonInterface() { }

		public void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new() {
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

		public T ReadFromJsonFile<T>(string filePath) where T : new() {
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
	}
}
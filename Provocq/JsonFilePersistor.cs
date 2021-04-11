using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Provocq
{
    public class JsonFilePersistor<T> : IPersistor<T>
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public JsonFilePersistor(string filePath)
        {
            _filePath = filePath;
            _jsonSerializerOptions = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve };
        }

        public T Load()
        {
            var dataString = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<T>(dataString, _jsonSerializerOptions);
        }

        public void Save(T dataContext)
        {
            var datastring = JsonSerializer.Serialize(dataContext, _jsonSerializerOptions);
            File.WriteAllText(_filePath, datastring);
        }
    }
}

using System.IO;
using Newtonsoft.Json;

namespace Common.Instrastructure.Helpers
{
    public static class JsonHelper
    {
        public static T GetFirstInstance<T>(string propertyName, string json)
        {
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType != JsonToken.PropertyName || (string) jsonReader.Value != propertyName)
                        continue;
                    jsonReader.Read();

                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<T>(jsonReader);
                }
                return default(T);
            }
        }
    }
}
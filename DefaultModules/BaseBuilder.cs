using Newtonsoft.Json;
using System.IO;

namespace DiscordBotFramework.DefaultModules
{
    public class BaseBuilder
    {
        public static object DeserailzeFromStream(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (StreamReader sr = new StreamReader(stream))
            using (JsonTextReader jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize(jsonTextReader);
            }
        }
    }
}

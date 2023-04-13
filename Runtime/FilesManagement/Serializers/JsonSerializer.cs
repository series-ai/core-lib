using System;
using Newtonsoft.Json;

namespace Padoru.Core.Files
{
    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings settings;

        public JsonSerializer()
        {
            settings = new JsonSerializerSettings();
        }
        
        public JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public void Serialize(object value, out string text)
        {
            text = JsonConvert.SerializeObject(value, settings);
        }

        public void Deserialize(Type type, ref string text, out object value)
        {
            value = JsonConvert.DeserializeObject(text, type, settings);
        }
    }
}
using System;
using System.Text;
using Newtonsoft.Json;

namespace Padoru.Core.Files
{
    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings settings;

        public JsonSerializer()
        {
            settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };
        }
        
        public JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public void Serialize(object value, out byte[] bytes)
        {
            var text = JsonConvert.SerializeObject(value, settings);
            
            bytes = Encoding.UTF8.GetBytes(text);
        }

        public void Deserialize(Type type, ref byte[] bytes, out object value)
        {
            var text = Encoding.UTF8.GetString(bytes);
            
            value = JsonConvert.DeserializeObject(text, type, settings);
        }
    }
}
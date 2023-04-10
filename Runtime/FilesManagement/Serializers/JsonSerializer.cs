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
            settings = new JsonSerializerSettings();
        }
        
        public JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public void Serialize(object value, out byte[] bytes)
        {
            bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, settings));
        }

        public void Deserialize(Type type, ref byte[] bytes, out object value)
        {
            value = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), type, settings);
        }
    }
}
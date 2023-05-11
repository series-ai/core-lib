using System;
using Newtonsoft.Json;

namespace Padoru.Core
{
    public class SubscribableValueJsonConverter<T> : JsonConverter<SubscribableValue<T>>
    {
        public override void WriteJson(JsonWriter writer, SubscribableValue<T> value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Value);
        }

        public override SubscribableValue<T> ReadJson(JsonReader reader, Type objectType, SubscribableValue<T> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var s = (T)reader.Value;

            if (existingValue != null)
            {
                existingValue.Value = s;
            }
            
            return existingValue ?? new SubscribableValue<T>(s);
        }
    }
}
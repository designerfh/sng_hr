using MongoDB.Bson;
using Newtonsoft.Json;
using System;

namespace snagajob_hr_app.Classes
{
    public class ObjectIdConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectId).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            return String.IsNullOrEmpty(value) ? ObjectId.Empty : new ObjectId(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}
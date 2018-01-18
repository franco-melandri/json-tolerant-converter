using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TolerantConverter
{
    public class CustomJsonConverter : JsonCreationConverter<CmComponentModel>
    {
        protected override CmComponentModel Create(Type objectType, JObject jObject)
        {
            var key = jObject["type"].Value<string>();
            return CmConstants.ModelsDictionary.ContainsKey(key)
                ? CmConstants.ModelsDictionary[key]()
                : UnknownCmComponentModel.Instance;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            var jObject = JObject.Load(reader);

            // Create target object based on JObject
            var target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
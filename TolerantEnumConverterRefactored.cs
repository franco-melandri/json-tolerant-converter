using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TolerantConverter
{
    public class TolerantEnumConverterRefactored<T> : JsonConverter
    {
        public static object GetDefaultValue(Type type) {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private Dictionary<string, object> _fromMap;

        public override bool CanConvert(Type objectType)
        {
            return objectType != null && objectType.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            InitMap(objectType);

            var enumText = reader.Value.ToString();
            var val = FromValue(objectType, enumText);

            System.Console.Error.WriteLine("val--------------> {0}", val );
            if (val != null)
                return val;

            var mydefault = GetDefaultValue(objectType);
                System.Console.Error.WriteLine("default--------------> {0}", mydefault );

            var names = Enum.GetNames(objectType);

            var unknownName = names
                .FirstOrDefault(n => string.Equals(n, Enum.GetName(objectType, mydefault), StringComparison.OrdinalIgnoreCase));

            if (unknownName == null)
                throw new JsonSerializationException(string.Format("Unable to parse '{0}' to enum {1}. Consider adding Unknown as fail-back value.", reader.Value,
                    objectType));

            return Enum.Parse(objectType, unknownName);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private void InitMap(Type enumType)
        {
            if (_fromMap != null)
                return;

            _fromMap = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                var key = field.GetEnumKey();
                _fromMap[key] = Enum.Parse(enumType, field.Name);
            }
        }

        private object FromValue(Type enumType, string value)
        {
            return !_fromMap.ContainsKey(value) ? null : _fromMap[value];
        }
    }

    public static class FieldInfoExtension 
    {
        public static string GetEnumKey(this FieldInfo field)
        {
            var enumMemberAttribute = field.GetCustomAttribute<EnumMemberAttribute>();
            return enumMemberAttribute != null ?
                    enumMemberAttribute.Value :
                    field.Name;         
        }
    }
}

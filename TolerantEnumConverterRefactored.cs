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
    public class TolerantEnumConverterRefactored : JsonConverter
    {

        public static object GetDefaultValue(Type type) {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private Dictionary<Type, Dictionary<string, object>> _fromValueMap; // string representation to Enum value map

        private Dictionary<Type, Dictionary<object, string>> _toValueMap; // Enum value to string map

        public override bool CanConvert(Type objectType)
        {
            var type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            return type != null && type.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var isNullable = IsNullableType(objectType);
            var enumType = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

            InitMap(enumType);

            if (reader.TokenType == JsonToken.String)
            {
                var enumText = reader.Value.ToString();

                var val = FromValue(enumType, enumText);

                System.Console.Error.WriteLine("val--------------> {0}", val );
                if (val != null)
                    return val;
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                var enumVal = Convert.ToInt32(reader.Value);

                var values = (int[]) Enum.GetValues(enumType);
                if (values.Contains(enumVal)) return Enum.Parse(enumType, enumVal.ToString());
            }

            if (isNullable) return null;
            var mydefault = GetDefaultValue(enumType);
                System.Console.Error.WriteLine("default--------------> {0}", mydefault );

            var names = Enum.GetNames(enumType);

            var unknownName = names
                .FirstOrDefault(n => string.Equals(n, Enum.GetName(enumType, mydefault), StringComparison.OrdinalIgnoreCase));

            if (unknownName == null)
                throw new JsonSerializationException(string.Format("Unable to parse '{0}' to enum {1}. Consider adding Unknown as fail-back value.", reader.Value,
                    enumType));

            return Enum.Parse(enumType, unknownName);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var enumType = value.GetType();

            InitMap(enumType);

            var val = ToValue(enumType, value);

            writer.WriteValue(val);
        }

        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private void InitMap(Type enumType)
        {
            if (_fromValueMap == null)
                _fromValueMap = new Dictionary<Type, Dictionary<string, object>>();

            if (_toValueMap == null)
                _toValueMap = new Dictionary<Type, Dictionary<object, string>>();

            if (_fromValueMap.ContainsKey(enumType))
                return; // already initialized

            var fromMap = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            var toMap = new Dictionary<object, string>();

            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                var name = field.Name;
                var enumValue = Enum.Parse(enumType, name);

                // use EnumMember attribute if exists
                var enumMemberAttribute = field.GetCustomAttribute<EnumMemberAttribute>();

                if (enumMemberAttribute != null)
                {
                    var enumMemberValue = enumMemberAttribute.Value;

                    fromMap[enumMemberValue] = enumValue;
                    toMap[enumValue] = enumMemberValue;
                }
                else
                {
                    toMap[enumValue] = name;
                }

                fromMap[name] = enumValue;
            }

            _fromValueMap[enumType] = fromMap;
            _toValueMap[enumType] = toMap;
        }

        private string ToValue(Type enumType, object obj)
        {
            var map = _toValueMap[enumType];

            return map[obj];
        }

        private object FromValue(Type enumType, string value)
        {
            var map = _fromValueMap[enumType];

            return !map.ContainsKey(value) ? null : map[value];
        }
    }
}

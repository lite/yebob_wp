using System;
using System.Collections.Generic;

namespace YebobDemo.Json
{
    public class JsonMapper
    {
        private IDictionary<Type, IJsonDeserializer> deserializers;
        private IDictionary<Type, IJsonSerializer> serializers;

        public JsonMapper()
        {
            this.deserializers = new Dictionary<Type, IJsonDeserializer>();
            this.serializers = new Dictionary<Type, IJsonSerializer>();
        }

        public void RegisterDeserializer(Type type, IJsonDeserializer deserializer)
        {
            this.deserializers[type] = deserializer;
        }

        public void RegisterSerializer(Type type, IJsonSerializer serializer)
        {
            this.serializers[type] = serializer;
        }

        public bool CanDeserialize(Type type)
        {
            if (type == typeof(JsonValue) ||
                this.deserializers.ContainsKey(type))
            {
                return true;
            }
            return false;
        }

        public bool CanSerialize(Type type)
        {
            if (type == typeof(JsonValue) ||
                this.serializers.ContainsKey(type))
            {
                return true;
            }
            return false;
        }

        public T Deserialize<T>(JsonValue value)
        {
            IJsonDeserializer deserializer;
            if (this.deserializers.TryGetValue(typeof(T), out deserializer))
            {
                return (T)deserializer.Deserialize(value, this);
            }
            throw new JsonException(String.Format("Could not find deserializer for type '{0}'.", typeof(T)));
        }

        public JsonValue Serialize(object obj)
        {
            Type objType = obj.GetType();
            IJsonSerializer serializer;
            if (this.serializers.TryGetValue(objType, out serializer))
            {
                return serializer.Serialize(obj, this);
            }
            throw new JsonException(String.Format("Could not find serializer for type '{0}'.", objType));
        }
    }
}


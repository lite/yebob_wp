using System;

namespace YebobDemo.Json
{
    public interface IJsonDeserializer
    {
        object Deserialize(JsonValue value, JsonMapper mapper);
    }
}

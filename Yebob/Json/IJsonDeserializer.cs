using System;

namespace Yebob.Json
{
    public interface IJsonDeserializer
    {
        object Deserialize(JsonValue value, JsonMapper mapper);
    }
}

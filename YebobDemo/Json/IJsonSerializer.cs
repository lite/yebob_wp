using System;

namespace YebobDemo.Json
{
    public interface IJsonSerializer
    {
        JsonValue Serialize(object obj, JsonMapper mapper);
    }
}

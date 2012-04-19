using System;

namespace Yebob.Json
{
    public interface IJsonSerializer
    {
        JsonValue Serialize(object obj, JsonMapper mapper);
    }
}

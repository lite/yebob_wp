using System;
using System.Collections;
using System.Collections.Generic;

using YebobDemo.Utils;

namespace YebobDemo.Json
{
    public class JsonObject : JsonValue
    {
        public JsonObject() :
            base(JsonValueType.Object, new Dictionary<string, JsonValue>())
        {
        }

        #region Public methods

        public override JsonValue GetValue(string name)
        {
            ArgumentUtils.AssertNotNull(name, "name");

            IDictionary<string, JsonValue> jsonObject = (IDictionary<string, JsonValue>)this.value;
            JsonValue result;
            if (jsonObject.TryGetValue(name, out result))
            {
                return result;
            }
            return null;
        }

        public override ICollection<JsonValue> GetValues()
        {
            return ((IDictionary<string, JsonValue>)this.value).Values;
        }

        public override bool ContainsName(string name)
        {
            return ((IDictionary<string, JsonValue>)this.value).ContainsKey(name);
        }

        public override ICollection<string> GetNames()
        {
            return ((IDictionary<string, JsonValue>)this.value).Keys;
        }

        public void AddValue(string name, JsonValue value)
        {
            ArgumentUtils.AssertNotNull(name, "name");
            ArgumentUtils.AssertNotNull(value, "value");

            IDictionary<string, JsonValue> jsonObject = (IDictionary<string, JsonValue>)this.value;
            if (jsonObject.ContainsKey(name))
            {
                throw new JsonException(String.Format(
                    "An entry with the name '{0}' already exists in the JSON object structure.",
                    name));
            }
            jsonObject.Add(name, value);
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;

using YebobDemo.Utils;

namespace YebobDemo.Json
{
    public class JsonArray : JsonValue
    {
        public JsonArray(params JsonValue[] values) :
            base(JsonValueType.Array, new List<JsonValue>(values))
        {
        }

        #region Public methods

        public override JsonValue GetValue(int index)
        {
            IList<JsonValue> jsonArray = (IList<JsonValue>)this.value;
            if (index < jsonArray.Count)
            {
                return jsonArray[index];
            }
            return null;
        }

        public override ICollection<JsonValue> GetValues()
        {
            return (IList<JsonValue>)this.value;
        }

        public void AddValue(JsonValue value)
        {
            ArgumentUtils.AssertNotNull(value, "value");

            ((IList<JsonValue>)this.value).Add(value);
        }

        #endregion
    }
}
using System;
using System.Runtime.Serialization;

namespace Yebob.Json
{
    public class JsonException : Exception
    {
        public JsonException()
        {
        }

        public JsonException(string message)
            : base(message)
        {
        }

        public JsonException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }
    }
}


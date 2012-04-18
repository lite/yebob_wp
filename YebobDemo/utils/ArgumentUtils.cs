using System;
using System.Globalization;

namespace YebobDemo.Utils
{
	public sealed class ArgumentUtils
	{
        public static void AssertNotNull(object argument, string name)
		{
			if (argument == null)
			{
				throw new ArgumentNullException (name,
                    String.Format(CultureInfo.InvariantCulture, 
                        "Argument '{0}' must not be null.", name));
			}
		}

        public static void AssertHasText(string argument, string name)
		{
			if (!StringUtils.HasText(argument))
			{
				throw new ArgumentNullException(name,
					String.Format (CultureInfo.InvariantCulture, 
                    "String argument '{0}' must have text; it must not be null, empty, or blank.", name));
			}
		}
	}
}
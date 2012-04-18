using System;

namespace YebobDemo.Utils
{
    internal sealed class StringUtils
    {
        internal static bool HasLength(string target)
        {
            return (target != null && target.Length > 0);
        }
        
        internal static bool HasText(string target)
        {
            if (target == null)
            {
                return false;
            }
            else
            {
                return HasLength(target.Trim());
            }
        }
    }
}
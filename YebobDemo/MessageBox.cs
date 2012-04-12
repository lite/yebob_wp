using System;
using System.Windows;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace YebobDemo
{
    public class MessageBox
    {
        public static void Show(string title, string text)
        {
            Show(title, text, new string[] { "OK" }, 0, MessageBoxIcon.None, null);
        }

        public static void Show(string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon, AsyncCallback callback)
        {
            if (Guide.IsVisible) return;
            Guide.BeginShowMessageBox(title, text, buttons, focusButton, icon, callback, null);

        }
    }
}

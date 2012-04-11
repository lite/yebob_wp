using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

class MessageBox
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
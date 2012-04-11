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
    public static int? Show(string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon, AsyncCallback callback)
    {
        // don't do anything if the guide is visible - one issue this handles is showing dialogs in quick
        // succession, we have to wait for the guide to go away before the next dialog can display
        if (Guide.IsVisible) return null;
        Guide.BeginShowMessageBox(title, text, buttons, focusButton, icon, callback, null);
        return null;
    }

}
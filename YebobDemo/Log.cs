using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace YebobDemo
{
    public class Log
    {
        public static void d(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }
        public static void e(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }
    }
}

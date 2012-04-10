using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Devices;
using Microsoft.Phone.Controls;

namespace Yebob
{
    public class NativeExecution
    {
        private readonly WebBrowser webBrowser;

        public NativeExecution(ref WebBrowser browser)
        {
            if (browser == null)
            {
                throw new ArgumentNullException("browser");
            }

            this.webBrowser = browser;
        }

        public static bool IsRunningOnEmulator()
        {
            return Microsoft.Devices.Environment.DeviceType == DeviceType.Emulator;
        }

        public void ProcessCommand(YebobCommandCall commandCallParams)
        {
            if (commandCallParams == null)
            {
                throw new ArgumentNullException("commandCallParams");
            }
        }
    }
}
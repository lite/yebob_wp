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
using System.Linq;

namespace Yebob
{
    /// <summary>
    /// Represents Cordova native command call: action callback, etc
    /// </summary>
    public class YebobCommandCall
    {
        public String Service { get; private set; }
        public String Action { get; private set; }
        public String CallbackId { get; private set; }
        public String Args { get; private set; }

        public static YebobCommandCall Parse(string commandStr)
        {
            if (string.IsNullOrEmpty(commandStr))
            {
                return null;
                //throw new ArgumentNullException("commandStr");
            }

            string[] split = commandStr.Split('/');
            if (split.Length < 3)
            {
                return null;
            }

            YebobCommandCall commandCallParameters = new YebobCommandCall();

            commandCallParameters.Service = split[0];
            commandCallParameters.Action = split[1];
            commandCallParameters.CallbackId = split[2];
            commandCallParameters.Args = split.Length <= 3 ? String.Empty : String.Join("/", split.Skip(3));

            // sanity check for illegal names
            // was failing with ::
            // CordovaCommandResult :: 1, Device1, {"status":1,"message":"{\"name\":\"XD.....
            if (commandCallParameters.Service.IndexOfAny(new char[] { '@', ':', ',', '!', ' ' }) > -1)
            {
                return null;
            }


            return commandCallParameters;
        }


        /// <summary>
        /// Private ctr to disable class creation.
        /// New class instance must be initialized via CordovaCommandCall.Parse static method.
        /// </summary>
        private YebobCommandCall() { }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBot.Plugin
{
    public class SkypeMessage
    {
        public string FromDisplayName { get; set; }
        public string FromHandle { get; set; }
        public string ChatName { get; set; }
        public string Message { get; set; }

        public bool SendMessages { get; set; }
        public bool MessageWasReceived { get; set; }
    }
}

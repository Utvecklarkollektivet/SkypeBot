using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkypeBot.Plugin;

namespace SkypeBot.HelloUserPlugin
{
    public class HelloPlugin : ISkypeBotPlugin
    {
        public IEnumerable<string> Hooks { get { return new string[] { "!hello", "!name" }; } }
        public bool GlobalHook { get { return false; } }

        public SkypeMessage Action(SkypeMessage skypeMessage)
        {
            string anwser = string.Format("Hello, {0}!", skypeMessage.FromDisplayName);
            skypeMessage.MessageWasReceived = false;
            skypeMessage.SendMessages = true;
            skypeMessage.Message = anwser;

            return skypeMessage;
        }
    }
}

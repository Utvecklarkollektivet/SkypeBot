using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkypeBot.Plugin;
using System.Diagnostics;

namespace SkypeBotDiePlugin
{
    public class Die : ISkypeBotPlugin
    {
        public IEnumerable<string> Hooks { get { return new string[] { "!die" }; } }
        public bool GlobalHook { get { return false; } }

        public SkypeMessage Action(SkypeMessage skypeMessage)
        {
            try
            {
                Process proc = Process.GetProcessesByName("SkypeBot.CMD").FirstOrDefault();
                proc.Kill();
            }
            catch (Exception ex)
            {
                skypeMessage.Message = ex.Message;
            }

            skypeMessage.MessageWasReceived = false;
            skypeMessage.SendMessages = true;

            return skypeMessage;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkypeBot.Plugin;

namespace SkypeBot.SpotifySearchPlugin
{
    public class SpotifySearchPlugin : ISkypeBotPlugin
    {
        public IEnumerable<string> Hooks { get { return new string[] { "!spotify", "!track" }; } }
        public bool GlobalHook { get { return false; } }

        public SkypeMessage Action(SkypeMessage skypeMessage)
        {
            string query = skypeMessage.Message.Replace("!spotify", "").Replace("!track", "");
            skypeMessage.Message = SpotifySearch.SearchTrack(query);
            skypeMessage.MessageWasReceived = false;
            skypeMessage.SendMessages = true;

            return skypeMessage;
        }
    }
}

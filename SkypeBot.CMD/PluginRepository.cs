using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using SkypeBot.Plugin;

namespace SkypeBot.CMD
{
    public class PluginRepository
    {
        [ImportMany("ISkypeBotPlugin")]
        public IEnumerable<ISkypeBotPlugin> Plugins { get; set; }
    }
}

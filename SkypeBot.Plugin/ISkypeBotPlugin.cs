using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace SkypeBot.Plugin
{
    [InheritedExport(typeof(ISkypeBotPlugin))]
    public interface ISkypeBotPlugin
    {
        IEnumerable<string> Hooks { get; }
        bool GlobalHook { get; }

        SkypeMessage Action(SkypeMessage skypeMessage);
    }
}

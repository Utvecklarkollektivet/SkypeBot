using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using SkypeBot.Plugin;
using SKYPE4COMLib;
using System.Threading;

namespace SkypeBot.CMD
{
    class Program
    {
        static Skype skype = new Skype();
        static PluginRepository pluginRepository;

        static void Main(string[] args)
        {
            skype.Attach(7, false);
            skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(skype_MessageStatus);

            pluginRepository = new PluginRepository()
            {
                Plugins = LoadPlugins()
            };

            while (true)
            {
                Thread.Sleep(500);
            }
        }

        static void skype_MessageStatus(ChatMessage msg, TChatMessageStatus status)
        {
            if (status == TChatMessageStatus.cmsReceived && msg.Body.StartsWith("!"))
            {
                SkypeMessage skypeMsg = new SkypeMessage()
                {
                    ChatName = msg.Chat.Name,
                    FromDisplayName = msg.FromDisplayName,
                    FromHandle = msg.FromHandle,
                    Message = msg.Body,
                    MessageWasReceived = true,
                    SendMessages = false
                };

                if (msg.Body.StartsWith("!"))
                {
                    string hook = msg.Body.Substring(0, msg.Body.IndexOf(' '));

                    pluginRepository.Plugins.Where(p => p.Hooks.Contains(hook)).ToList().ForEach(p =>
                    {
                        SkypeMessage sm = p.Action(skypeMsg);
                        if (sm.SendMessages)
                            SendSkypeMessage(sm.ChatName, sm.Message);
                    });
                }

                pluginRepository.Plugins.Where(p => p.GlobalHook == true).ToList().ForEach(p =>
                {
                    SkypeMessage sm = p.Action(skypeMsg);
                    if (sm.SendMessages)
                        SendSkypeMessage(sm.ChatName, sm.Message);
                });
            }
        }

        static void SendSkypeMessage(string chatName, string message)
        {
            IChat chat = skype.get_Chat(chatName);
            chat.SendMessage(message);
        }

        static IEnumerable<ISkypeBotPlugin> LoadPlugins()
        {
            IEnumerable<ISkypeBotPlugin> plugins;

            try
            {
                var catalog = new DirectoryCatalog(@"Plugins");
                var contaioner = new CompositionContainer(catalog);
                plugins =  contaioner.GetExportedValues<ISkypeBotPlugin>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            Console.WriteLine("Number of plugins loaded: " + plugins.Count());
            return plugins;
        }
    }
}

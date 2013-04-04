using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SKYPE4COMLib;
using Newtonsoft.Json;
using SkypeBot.Models;
using SkypeBot.Helpers;
using System.Data;

namespace SkypeBot
{
    class Program
    {
        static SkypeBot.Models.Settings settings = new SkypeBot.Models.Settings();
        static Skype skype = new Skype();

        static void Main(string[] args)
        {
            settings = SettingsManager.LoadSettings();
            SettingsManager.Print(settings);

            skype.Attach(7, false);
            skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(skype_MessageStatus);

            while (!settings.Die)
            {
                Thread.Sleep(1000);
            }
        }

        static void SendSkypeMessage(string message, string chatName)
        {
            IChat chat = skype.get_Chat(chatName);
            chat.SendMessage(message);
        }

        static void skype_MessageStatus(ChatMessage msg, TChatMessageStatus status)
        {
            if (status == TChatMessageStatus.cmsReceived)
            {
                SkypeMessage sMsg = new SkypeMessage();
                sMsg.ChatName = msg.Chat.Name;
                sMsg.From = msg.FromDisplayName;

                if (msg.Body.IndexOf(settings.Trigger) == 0)
                {
                    string rawcmd = msg.Body.Remove(0, settings.Trigger.Length);
                    string cmd = msg.Body.Remove(0, settings.Trigger.Length).ToLower();
                    ParseCMD(rawcmd, cmd, sMsg);
                }
                else if (settings.SpotifyToYoutube)
                {
                    if (msg.Body.StartsWith("http://open.spotify.com/track/"))
                    {
                        string track = Spotify.GetTrackName(msg.Body.Replace("http://open.spotify.com/track/", ""));
                        
                        if(!string.IsNullOrEmpty(track))
                        {
                            GoogleSearch("youtube" + track, sMsg);
                            SendSkypeMessage(track, sMsg.ChatName);
                        }
                    }
                }
            }
        }

        static void ParseCMD(string rawcmd, string cmd, SkypeMessage sMsg)
        {
            string[] args = cmd.Split(' ');
            string result = "";

            switch (args[0])
            {
                case "ask":
                    GoogleSearch(cmd.Remove(0, args[0].Length), sMsg);
                    break;

                case "help":
                    result = FAQ.GetHelp();
                    break;

                case "track":
                    result = Spotify.SearchTrack(cmd.Remove(0, args[0].Length));
                    break;

                case "track5":
                    result = Spotify.SearchTrack(cmd.Remove(0, args[0].Length), 5);
                    break;

                case "math":
                    var math = new DataTable().Compute(cmd.Remove(0, args[0].Length), null);
                    result = math.ToString();
                    break;

                case "snippet":
                    settings.IncludeSnippet = !settings.IncludeSnippet;
                    SettingsManager.Save(settings);
                    SettingsManager.Print(settings);
                    result = "Will include text snippet with !ask = " + settings.IncludeSnippet.ToString();
                    break;

                case "googlekey":
                    settings.GoogleKey = rawcmd.Remove(0, args[0].Length);
                    SettingsManager.Save(settings);
                    SettingsManager.Print(settings);
                    result = "Google API Key is now: " + settings.GoogleKey;
                    break;

                case "googlecx":
                    settings.GoogleCX = rawcmd.Remove(0, args[0].Length);
                    SettingsManager.Save(settings);
                    SettingsManager.Print(settings);
                    result = "Google CX value is now: " + settings.GoogleCX;
                    break;

                case "spotify2youtube":
                    settings.SpotifyToYoutube = !settings.SpotifyToYoutube;
                    SettingsManager.Save(settings);
                    SettingsManager.Print(settings);
                    result = "Will try to convert Spotify links to Youtube links = " + settings.SpotifyToYoutube.ToString();
                    break;

                case "die":
                    settings.Die = !settings.Die;
                    result = sMsg.From + ", I will remeber this! Bye...";
                    break;
            }

            if (!string.IsNullOrEmpty(result))
            {
                SendSkypeMessage(result, sMsg.ChatName);
            }
        }

        static void GoogleSearch(string query, SkypeMessage sMSg)
        {
            string result = "";
            GoogleSearch search = new GoogleSearch()
            {
                Key = settings.GoogleKey,
                CX = settings.GoogleCX
            };

            search.SearchCompleted += (a, b) =>
            {
                StringBuilder s = new StringBuilder();

                if (settings.IncludeSnippet)
                {
                    s.Append(b.Response.Items[0].Snippet + Environment.NewLine + b.Response.Items[0].Link);
                }
                else
                {
                    s.Append(b.Response.Items[0].Link);
                }
                
                result = s.ToString();
                SendSkypeMessage(result, b.SkypeMessage.ChatName);
            };

            if (!string.IsNullOrEmpty(settings.GoogleKey) && !string.IsNullOrEmpty(settings.GoogleCX))
            {
                search.Search(query, sMSg);
            }
            else
            {
                string s = sMSg.From + ", you must first set Google API Key and Google CX value before you can ask me things. See !help.";
                SendSkypeMessage(s, sMSg.ChatName);
            }
            
        }
    }
}

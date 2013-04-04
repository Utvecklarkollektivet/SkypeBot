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
        static bool creatingEvent = false;
        static Event tempEvent = null;

        static void Main(string[] args)
        {
            settings = SettingsManager.LoadSettings();
            SettingsManager.Print(settings);

            skype.Attach(7, false);
            skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(skype_MessageStatus);

            int index = 0;
            while (!settings.Die)
            {
                if (index == 60)
                {
                    EventsCollection events = new EventsCollection();
                    events = EventManager.GetEvents();
                    if (events.Events != null)
                    {
                        events.Events.Where(e => e.DateTime.ToShortDateString() == DateTime.Now.ToShortDateString() && e.DateTime.ToShortTimeString() == DateTime.Now.ToShortTimeString()).ToList().ForEach(e =>
                            {
                                StringBuilder s = new StringBuilder();
                                s.AppendLine("======== EVENT ========");
                                s.AppendLine(e.Text);
                                s.AppendLine("");
                                s.AppendLine("Created by: " + e.CreatedBy);
                                s.AppendLine("======== /EVENT ========");

                                SendSkypeMessage(s.ToString(), e.ChatName);
                            });
                    }

                    index = 0;
                }
                else
                {
                    index++;
                }

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
                else if (msg.Body.StartsWith("http://open.spotify.com/track/"))
                {
                    if (settings.SpotifyToYoutube)
                    {
                        string track = Spotify.GetTrackName(msg.Body.Replace("http://open.spotify.com/track/", ""));

                        if (!string.IsNullOrEmpty(track))
                        {
                            GoogleSearch("youtube" + track, sMsg, false, track);
                        }
                    }
                }
                else if (creatingEvent && msg.FromDisplayName == tempEvent.CreatedBy)
                {
                    tempEvent.Text = msg.Body;
                    EventManager.Add(tempEvent);

                    StringBuilder s = new StringBuilder();
                    s.AppendLine("New Event Created");
                    s.AppendLine("Created by: " + tempEvent.CreatedBy);
                    s.AppendLine(tempEvent.DateTime.ToString());
                    s.AppendLine("");
                    s.AppendLine(tempEvent.Text);

                    SendSkypeMessage(s.ToString(), sMsg.ChatName);
                    creatingEvent = false;
                    tempEvent = null;
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
                    GoogleSearch(cmd.Remove(0, args[0].Length), sMsg, settings.IncludeSnippet, null);
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

                case "event":
                    try
                    {
                        tempEvent = new Event();
                        tempEvent.CreatedBy = sMsg.From;
                        //tempEvent.DateTime = new DateTime();
                        tempEvent.DateTime = DateTime.Parse(cmd.Remove(0, args[0].Length));
                        tempEvent.ChatName = sMsg.ChatName;
                        result = "Okey, " + sMsg.From + ". Next message from you will store the content of this new event, that will be displayd: + " +tempEvent.DateTime.ToString();
                        creatingEvent = true;
                    }
                    catch(Exception e)
                    {
                        string s = e.Message;
                    }
                    
                    break;
            }

            if (!string.IsNullOrEmpty(result))
            {
                SendSkypeMessage(result, sMsg.ChatName);
            }
        }

        static void GoogleSearch(string query, SkypeMessage sMSg, bool includeSnippet, string extra)
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

                if (includeSnippet)
                {
                    s.Append(b.Response.Items[0].Snippet + Environment.NewLine + b.Response.Items[0].Link);
                }
                else
                {
                    if (string.IsNullOrEmpty(extra))
                    {
                        s.Append(b.Response.Items[0].Link);
                    }
                    else
                    {
                        s.Append(extra + Environment.NewLine + b.Response.Items[0].Link);
                    }

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

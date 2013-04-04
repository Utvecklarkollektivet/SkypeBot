using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SkypeBot.Models;
using System.IO;

namespace SkypeBot.Helpers
{
    public static class SettingsManager
    {
        public static Settings LoadSettings()
        {
            if (File.Exists("skypebot.bot"))
            {
                Settings s = new Settings();
                Stream stream = File.Open("skypebot.bot", FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();

                s = (Settings)bf.Deserialize(stream);
                stream.Close();

                return s;
            }
            else
            {
                Settings s = new Settings();
                s.Trigger = "!";
                s.SpotifyToYoutube = true;
                s.GoogleKey = "";
                s.GoogleCX = "";
                s.GoogleSearch = true;
                s.IncludeSnippet = true;
                s.Listen = true;
                s.Die = false;

                Stream stream = File.Open("skypebot.bot", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(stream, s);
                stream.Close();

                return s;
            }
        }

        public static void Save(Settings settings)
        {
            Stream stream = File.Open("skypebot.bot", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(stream, settings);
            stream.Close();
        }

        public static void Print(Settings settings)
        {
            Console.WriteLine("Settings");
            Console.WriteLine("--------");
            Console.WriteLine("     Trigger: '" + settings.Trigger + "'");
            Console.WriteLine("     Listen: " + settings.Listen.ToString());
            Console.WriteLine("     Google Key: " + settings.GoogleKey);
            Console.WriteLine("     Google CX: " + settings.GoogleCX);
            Console.WriteLine("     Google Search: " + settings.GoogleSearch.ToString());
            Console.WriteLine("     Google Search - Include Snippet: " + settings.IncludeSnippet.ToString());
            Console.WriteLine("     Spotify to Youtube: " + settings.SpotifyToYoutube.ToString());
            Console.WriteLine("--------");
            Console.WriteLine("");
        }
    }
}

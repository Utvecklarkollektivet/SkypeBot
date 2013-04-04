using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkypeBot.Models.Spotify.Track;
using System.Net;
using Newtonsoft.Json;

namespace SkypeBot.Helpers
{
    public static class Spotify
    {
        public static string SearchTrack(string q)
        {
            string result = null;

            try
            {
                WebClient client = new WebClient();
                string json = client.DownloadString("http://ws.spotify.com/search/1/track.json?q=" + q);
                var resObj = JsonConvert.DeserializeObject<SpotifyTrack>(json);

                result = resObj.tracks[0].artists[0].name + " - " + resObj.tracks[0].name + Environment.NewLine + "http://open.spotify.com/track/" + resObj.tracks[0].href.Replace("spotify:track:", "");
            }
            catch
            {

            }

            return result;
        }

        public static string SearchTrack(string q, int num)
        {
            string result = null;

            try
            {
                WebClient client = new WebClient();
                string json = client.DownloadString("http://ws.spotify.com/search/1/track.json?q=" + q);
                var resObj = JsonConvert.DeserializeObject<SpotifyTrack>(json);

                string temp = "";

                for (int i = 0; i < num; i++)
                {
                    temp += resObj.tracks[i].artists[0].name + " - " + resObj.tracks[i].name + Environment.NewLine + "http://open.spotify.com/track/" + resObj.tracks[i].href.Replace("spotify:track:", "") + Environment.NewLine;
                }
                result = temp;
            }
            catch
            {

            }

            return result;
        }

        public static string GetTrackName(string q)
        {
            string result = null;

            try
            {
                WebClient client = new WebClient();
                string json = client.DownloadString("http://ws.spotify.com/lookup/1/.json?uri=spotify:track:" + q);
                var resObj = JsonConvert.DeserializeObject<SkypeBot.Models.Spotify2YoutubeTrack.Spotify2YoutubeTrack>(json);

                result = resObj.track.artists[0].name + " - " + resObj.track.name;
            }
            catch
            {

            }

            return result;
        }
    }
}

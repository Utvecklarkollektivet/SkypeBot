using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace SkypeBot.SpotifySearchPlugin
{
    public static class SpotifySearch
    {
        public static string SearchTrack(string query)
        {
            string result = "No tracks were found.";

            try
            {
                WebClient client = new WebClient();
                string json = client.DownloadString("http://ws.spotify.com/search/1/track.json?q=" + query);
                var resObj = JsonConvert.DeserializeObject<SpotifyTrack>(json);
                result = resObj.tracks[0].artists[0].name + " - " + resObj.tracks[0].name + Environment.NewLine + "http://open.spotify.com/track/" + resObj.tracks[0].href.Replace("spotify:track:", "");
            }
            catch
            {

            }

            return result;
        }
    }
}

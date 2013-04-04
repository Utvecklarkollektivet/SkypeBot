using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBot.Models.Spotify2YoutubeTrack
{
    public class Album
    {
        public string released { get; set; }
        public string href { get; set; }
        public string name { get; set; }
    }

    public class ExternalId
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public class Artist
    {
        public string href { get; set; }
        public string name { get; set; }
    }

    public class Availability
    {
        public string territories { get; set; }
    }

    public class Track
    {
        public bool available { get; set; }
        public Album album { get; set; }
        public string __invalid_name__track_number { get; set; }
        public string popularity { get; set; }
        public List<ExternalId> __invalid_name__external_ids { get; set; }
        public double length { get; set; }
        public string href { get; set; }
        public List<Artist> artists { get; set; }
        public Availability availability { get; set; }
        public string name { get; set; }
    }

    public class Info
    {
        public string type { get; set; }
    }

    public class Spotify2YoutubeTrack
    {
        public Track track { get; set; }
        public Info info { get; set; }
    }
}

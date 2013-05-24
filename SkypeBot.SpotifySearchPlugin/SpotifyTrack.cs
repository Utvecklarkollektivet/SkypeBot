using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBot.SpotifySearchPlugin
{
    public class Info
    {
        public int num_results { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public string query { get; set; }
        public string type { get; set; }
        public int page { get; set; }
    }

    public class Availability
    {
        public string territories { get; set; }
    }

    public class Album
    {
        public string released { get; set; }
        public string href { get; set; }
        public string name { get; set; }
        public Availability availability { get; set; }
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

    public class Track
    {
        public Album album { get; set; }
        public string name { get; set; }
        public string popularity { get; set; }
        public List<ExternalId> __invalid_name__external_ids { get; set; }
        public double length { get; set; }
        public string href { get; set; }
        public List<Artist> artists { get; set; }
        public string __invalid_name__track_number { get; set; }
    }

    public class SpotifyTrack
    {
        public Info info { get; set; }
        public List<Track> tracks { get; set; }
    }
}

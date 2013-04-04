using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBot.Models
{
    [Serializable]
    public class Settings
    {
        public string Trigger { get; set; }
        public bool SpotifyToYoutube { get; set; }
        public string GoogleKey { get; set; }
        public string GoogleCX { get; set; }
        public bool GoogleSearch { get; set; }
        public bool IncludeSnippet { get; set; }
        public bool Listen { get; set; }
        public bool Die { get; set; }

    }
}

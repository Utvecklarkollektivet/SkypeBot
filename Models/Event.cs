using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBot.Models
{
    [Serializable]
    public class EventsCollection
    {
        public List<Event> Events { get; set; }
    }

    [Serializable]
    public class Event
    {
        public DateTime DateTime { get; set; }
        public string CreatedBy { get; set; }
        public string Text { get; set; }
        public string ChatName { get; set; }
    }
}

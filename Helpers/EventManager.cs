using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SkypeBot.Models;
using System.Runtime.Serialization.Formatters.Binary;

namespace SkypeBot.Helpers
{
    public static class EventManager
    {
        private static EventsCollection ReadCollection()
        {
            EventsCollection events = new EventsCollection();

            if (File.Exists("events.bot"))
            {
                Stream stream = File.Open("events.bot", FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();

                events = (EventsCollection)bf.Deserialize(stream);
                stream.Close();

                return events;
            }

            events.Events = new List<Event>();

            return events;
        }

        private static void SaveCollection(EventsCollection e)
        {
            Stream stream = File.Open("events.bot", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(stream, e);
            stream.Close();
        }

        public static void Add(Event e)
        {
            EventsCollection events = new EventsCollection();
            events = ReadCollection();
            events.Events.Add(e);
            SaveCollection(events);
        }

        public static EventsCollection GetEvents()
        {
            EventsCollection events = new EventsCollection();
            events = ReadCollection();
            return events;
        }

    }
}

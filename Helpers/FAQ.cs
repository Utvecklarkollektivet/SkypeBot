using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBot.Helpers
{
    public static class FAQ
    {
        public static string GetHelp()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("#");
            s.AppendLine("!ask");
            s.AppendLine("      Search using Google search.");
            s.AppendLine("      Turn off Include Snippet if you only want the link retuned.");
            s.AppendLine("      Example: !ask hello world program");

            s.AppendLine("");
            s.AppendLine("!math");
            s.AppendLine("      To lazy for math?");
            s.AppendLine("      Example: !math (5+5)*2");

            s.AppendLine("");
            s.AppendLine("!track");
            s.AppendLine("      Search for music on Spotify");
            s.AppendLine("      Example: !track rihanna diamonds");

            s.AppendLine("");
            s.AppendLine("!track5");
            s.AppendLine("      The same as !track but will return 5 results");

            s.AppendLine("");
            s.AppendLine("!snippet");
            s.AppendLine("      Toggle the value of including text snippet when result comes back from !ask.");
            s.AppendLine("      Example: !snippet");


            s.AppendLine("");
            s.AppendLine("!googlekey");
            s.AppendLine("      Sets the Google API Key.");
            s.AppendLine("      Example: !googlekey Akfsd234342jkadfjjk");


            s.AppendLine("");
            s.AppendLine("!googlecx");
            s.AppendLine("      Sets the Google CX value.");
            s.AppendLine("      Example: !googlecx Akfsd234342jkadfjjk");

            s.AppendLine("");
            s.AppendLine("!spotify2youtube");
            s.AppendLine("      Toggle the value of converting Spotify links to Youtube links.");
            s.AppendLine("      Example: !spotify2youtube");

            s.AppendLine("");
            s.AppendLine("!die");
            s.AppendLine("      I will die, so please, do not use this command.");
            s.AppendLine("      Example: !die");

            return s.ToString();
        }
    }
}

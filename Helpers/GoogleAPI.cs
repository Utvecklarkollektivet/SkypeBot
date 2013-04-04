using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using SKYPE4COMLib;
using SkypeBot.Models;

namespace SkypeBot.Helpers
{

    //
    // This class is little modified from its source
    // http://kiwigis.blogspot.se/2011/03/google-custom-search-in-c.html
    //
    public class GoogleSearch
    {
        public GoogleSearch()
        {
            this.Num = 10;
            this.Start = 1;
            this.SafeLevel = SafeLevel.off;
        }
        //
        // PROPERTIES
        //
        public string Key { get; set; }
        public string CX { get; set; }
        public int Num { get; set; }
        public int Start { get; set; }
        public SafeLevel SafeLevel { get; set; }
        //
        // EVENTS
        //
        public event EventHandler<SearchEventArgs> SearchCompleted;
        //
        // METHODs
        //
        protected void OnSearchCompleted(SearchEventArgs e)
        {
            if (this.SearchCompleted != null)
            {
                this.SearchCompleted(this, e);
            }
        }
        public void Search(string search, SkypeMessage sMsg)
        {
            // Check Parameters
            if (string.IsNullOrWhiteSpace(this.Key))
            {
                throw new Exception("Google Search 'Key' cannot be null");
            }
            if (string.IsNullOrWhiteSpace(this.CX))
            {
                throw new Exception("Google Search 'CX' cannot be null");
            }
            if (string.IsNullOrWhiteSpace(search))
            {
                throw new ArgumentNullException("search");
            }
            if (this.Num < 0 || this.Num > 10)
            {
                throw new ArgumentNullException("Num must be between 1 and 10");
            }
            if (this.Start < 1 || this.Start > 100)
            {
                throw new ArgumentNullException("Start must be between 1 and 100");
            }

            // Build Query
            string query = string.Empty;
            query += string.Format("q={0}", search);
            query += string.Format("&key={0}", this.Key);
            query += string.Format("&cx={0}", this.CX);
            query += string.Format("&safe={0}", this.SafeLevel.ToString());
            query += string.Format("&alt={0}", "json");
            query += string.Format("&num={0}", this.Num);
            query += string.Format("&start={0}", this.Start);

            // Construct URL
            UriBuilder builder = new UriBuilder()
            {
                Scheme = Uri.UriSchemeHttps,
                Host = "www.googleapis.com",
                Path = "customsearch/v1",
                Query = query
            };

            // Submit Request
            WebClient w = new WebClient();
            w.Encoding = Encoding.GetEncoding("UTF-8"); ;
            w.DownloadStringCompleted += (a, b) =>
            {
                // Check for errors
                if (b == null) { return; }
                if (b.Error != null) { return; }
                if (string.IsNullOrWhiteSpace(b.Result)) { return; }

                // Desearealize from JSON to .NET objects
                Byte[] bytes = Encoding.Unicode.GetBytes(b.Result);
                MemoryStream memoryStream = new MemoryStream(bytes);
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GoogleSearchResponse));
                GoogleSearchResponse googleSearchResponse = dataContractJsonSerializer.ReadObject(memoryStream) as GoogleSearchResponse;
                memoryStream.Close();

                // Raise Event
                this.OnSearchCompleted(
                    new SearchEventArgs()
                    {
                        Response = googleSearchResponse,
                        SkypeMessage = sMsg
                    }
                );
            };
            w.DownloadStringAsync(builder.Uri);

        }
    }

    public enum SafeLevel { off, medium, high }

    public class SearchEventArgs : EventArgs
    {
        public GoogleSearchResponse Response { get; set; }
        public SkypeMessage SkypeMessage { get; set; }
    }

    [DataContract]
    public class GoogleSearchResponse
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "url")]
        public Url Url { get; set; }
        [DataMember(Name = "queries")]
        public Queries Queries { get; set; }
        [DataMember(Name = "context")]
        public Context Context { get; set; }
        [DataMember(Name = "items")]
        public List<Item> Items { get; set; }
    }

    [DataContract]
    public class Url
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "template")]
        public string Template { get; set; }
    }

    [DataContract]
    public class Queries
    {
        [DataMember(Name = "nextPage")]
        public List<Page> NextPage { get; set; }
        [DataMember(Name = "request")]
        public List<Page> Request { get; set; }
    }

    [DataContract]
    public class Page
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "totalResults")]
        public int Request { get; set; }
        [DataMember(Name = "searchTerms")]
        public string SearchTerms { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "startIndex")]
        public int StartIndex { get; set; }
        [DataMember(Name = "inputEncoding")]
        public string InputEncoding { get; set; }
        [DataMember(Name = "outputEncoding")]
        public string OutputEncoding { get; set; }
        [DataMember(Name = "safe")]
        public string Safe { get; set; }
        [DataMember(Name = "cx")]
        public string CX { get; set; }
    }

    [DataContract]
    public class Context
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "facets")]
        public List<List<Facet>> Facets { get; set; }
    }

    [DataContract]
    public class Facet
    {
        [DataMember(Name = "label")]
        public string Label { get; set; }
        [DataMember(Name = "anchor")]
        public string Anchor { get; set; }
    }

    [DataContract]
    public class Item
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "htmlTitle")]
        public string HtmlTitle { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "displayLink")]
        public string DisplayLink { get; set; }
        [DataMember(Name = "snippet")]
        public string Snippet { get; set; }
        [DataMember(Name = "htmlSnippet")]
        public string HtmlSnippet { get; set; }
        [DataMember(Name = "cacheId")]
        public string CacheId { get; set; }
        //[DataMember(Name = "pagemap")]        *** Cannot deserialize JSON to .NET! ***
        //public Pagemap Pagemap { get; set; }
    }

    [DataContract]
    public class Pagemap
    {
        [DataMember(Name = "metatags")]
        public List<Dictionary<string, string>> Metatags { get; set; }
    }

    [DataContract]
    public class Metatag
    {
        [DataMember(Name = "creationdate")]
        public string Creationdate { get; set; }
        [DataMember(Name = "moddate")]
        public string Moddate { get; set; }
    }
}

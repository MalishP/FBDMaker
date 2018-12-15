using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FBDMaker
{
    public class GoogleBooks
    {

        private Item GBook;
        public static List<GoogleBooks> GetListGoogleBooks(string JsonString)
        {
            List<GoogleBooks> ListGBooks = new List<GoogleBooks>();
            JObject GoogleSearchResult = JObject.Parse(JsonString);

            // get JSON result objects into a list
            int a = (int)GoogleSearchResult["totalItems"];
            if (a > 0)
            {
                IList<JToken> results = GoogleSearchResult["items"].Children().ToList();
                // serialize JSON results into .NET objects
               // IList<Item> searchResults = new List<Item>();
                foreach (JToken result in results)
                {
                    // JToken.ToObject is a helper method that uses JsonSerializer internally
                    Item searchResult = result.ToObject<Item>();
                    ListGBooks.Add(new GoogleBooks(searchResult));
                }
            }
            return ListGBooks;
        }
        private GoogleBooks(Item GBookItem)
        {
            GBook = GBookItem;
        }

      
        public string GoogleID { get => GBook.Id; }
        public Uri GoogleLink { get => GBook.SelfLink; }
        public string Title { get => GBook.VolumeInfo.Title; }
        public List<string> AuthorsList { get => GBook.VolumeInfo.Authors; }
        public string Authors { get => GBook.VolumeInfo.Authors == null?string.Empty:string.Join(", ", GBook.VolumeInfo.Authors.Select(h => string.IsNullOrWhiteSpace(h)?string.Empty:h)); }
        public string Publisher { get=> string.IsNullOrWhiteSpace(GBook.VolumeInfo.Publisher)?string.Empty: GBook.VolumeInfo.Publisher; }
        public string PublishedDate { get => string.IsNullOrWhiteSpace(GBook.VolumeInfo.PublishedDate) ? string.Empty : GBook.VolumeInfo.PublishedDate; }
        public string Description { get => string.IsNullOrWhiteSpace(GBook.VolumeInfo.Description) ? string.Empty : GBook.VolumeInfo.Description; }
        public string ISBN { get=> GBook.VolumeInfo.IndustryIdentifiers==null?string.Empty:string.Join(", ", GBook.VolumeInfo.IndustryIdentifiers.Select(i=>i.Type+":"+i.Identifier)); }
        public int? PageCount { get => GBook.VolumeInfo.PageCount.HasValue?(int)GBook.VolumeInfo.PageCount:0; }
        public string Categories { get => GBook.VolumeInfo.Categories==null? string.Empty: string.Join(", ", GBook.VolumeInfo.Categories.Select(h => string.IsNullOrWhiteSpace(h) ? string.Empty : h)); }
        public Uri ImageLinks { get => GBook.VolumeInfo.ImageLinks!=null?GBook.VolumeInfo.ImageLinks.Thumbnail: new Uri("dat\\unnamed.jpg", UriKind.Relative); }
        public string Language { get => string.IsNullOrWhiteSpace(GBook.VolumeInfo.Language) ? string.Empty : GBook.VolumeInfo.Language; }

        public string PublishInfo { get => (string.IsNullOrWhiteSpace(GBook.VolumeInfo.Publisher) ? string.Empty : GBook.VolumeInfo.Publisher) + " " 
                                         + (string.IsNullOrWhiteSpace(GBook.VolumeInfo.PublishedDate) ? string.Empty : GBook.VolumeInfo.PublishedDate) + " " 
                                          + GBook.VolumeInfo.PageCount.ToString(); }

    }
    public partial class Item
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("selfLink")]
        public Uri SelfLink { get; set; }

        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }


    }


    public partial class VolumeInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("industryIdentifiers")]
        public List<IndustryIdentifier> IndustryIdentifiers { get; set; }

        [JsonProperty("pageCount", NullValueHandling = NullValueHandling.Ignore)]
        public long? PageCount { get; set; }

        [JsonProperty("printType")]
        public string PrintType { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("imageLinks")]
        public ImageLinks ImageLinks { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }


    }

    public partial class ImageLinks
    {
        [JsonProperty("smallThumbnail")]
        public Uri SmallThumbnail { get; set; }

        [JsonProperty("thumbnail")]
        public Uri Thumbnail { get; set; }
    }

    public partial class IndustryIdentifier
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }

}

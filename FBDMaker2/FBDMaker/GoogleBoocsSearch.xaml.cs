using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.IO;


namespace FBDMaker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class GBooksSearch : Window
    {
        public bool FindTitle { get; set; } = false;
        public bool FindAuthor { get; set; } = false;
        public bool FindISBN { get; set; } = false;
        public string Title2Find { get; set; } = string.Empty;
        public string Author2Find { get; set; } = string.Empty;
        public string ISBN2Find { get; set; } = string.Empty;


        public bool isReturnTitle { get; set; } = true;
        public bool isReturnAuthors { get; set; } = true;
        public bool isReturnPublisher { get; set; } = true;
        public bool isReturnDate { get; set; } = true;
        public bool isReturnDescription { get; set; } = true;
        public bool isReturnISBN { get; set; } = true;
        public bool isReturnPageCount { get; set; } = true;
        public bool isReturnCategories { get; set; } = true;
        public bool isReturnImage { get; set; } = true;

        private FBDInf Book;
       // private List<GoogleBooks> _resGBook = new List<GoogleBooks>();
        private static readonly HttpClient client = new HttpClient();

        public ObservableCollection<GoogleBooks> ResGBook { get; set; } = new ObservableCollection<GoogleBooks>();
        public GBooksSearch(FBDInf BI)
        {
            Book = BI;
            Title2Find = Book.Book.Title;
            FindTitle = !string.IsNullOrWhiteSpace(Title2Find);
            Author2Find = string.Join(" ", Book.Book.Avtor.List.Select(a => a.ListName));
            FindAuthor = !string.IsNullOrWhiteSpace(Author2Find);
            ISBN2Find = Book.Publisher.ISBN;
            FindISBN = !string.IsNullOrWhiteSpace(ISBN2Find);
            client.BaseAddress = new Uri("https://www.googleapis.com/");
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Query_string = (FindTitle && !string.IsNullOrWhiteSpace(Title2Find) ? "intitle:" + Title2Find + " " : string.Empty)
                                + (FindAuthor && !string.IsNullOrWhiteSpace(Author2Find) ? "inauthor:" + Author2Find + " " : string.Empty)
                                + (FindISBN && !string.IsNullOrWhiteSpace(ISBN2Find) ? "isbn:" + ISBN2Find + " " : string.Empty);


            string rezJSon = GetFromGoogle(Query_string);//.Result;
            List<GoogleBooks> rezlist = GoogleBooks.GetListGoogleBooks(rezJSon);
            if (rezlist.Count > 0)
            {
                //ResGBook = (ObservableCollection<GoogleBooks>)rezlist;
                ResGBook.Clear();
                foreach (GoogleBooks gb in rezlist)
                { ResGBook.Add(gb); }
                //ResGBook.Add()
                //    AddRange(rezlist);
            }
            else
            {
                MessageBox.Show("Насяльника нисего не нашел!!!", "Поиск в google");
            }
        }

        private static async Task<List<GoogleBooks>> getGoogleData(String G_Query)
        {
            var stringTask = client.GetStringAsync("/books/v1/volumes?q=" + G_Query);
            var msg = await stringTask;
           
            return GoogleBooks.GetListGoogleBooks(msg);

        }

        private string GetFromGoogle(string G_Query)
        {
            WebRequest request =
                   WebRequest.Create("https://www.googleapis.com/books/v1/volumes?maxResults=40&q=" + Uri.EscapeDataString(G_Query));
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GoogleBooks rez = (GoogleBooks)ListGB.SelectedItem;
            if (rez != null)
            {
                Book.Book.Title = !string.IsNullOrWhiteSpace(rez.Title) && isReturnTitle ? rez.Title : Book.Book.Title;
                if (isReturnAuthors && !string.IsNullOrWhiteSpace(rez.Authors))
                {
                    Book.Book.Avtor.ParseString(rez.Authors);
                }

                Book.Publisher.Publisher = !string.IsNullOrWhiteSpace(rez.Publisher) && isReturnPublisher ? rez.Publisher : Book.Publisher.Publisher;
                Book.Book.s_BookDate = !string.IsNullOrWhiteSpace(rez.PublishedDate) && isReturnDate ? rez.PublishedDate : Book.Book.s_BookDate;
                Book.Book.Annot = !string.IsNullOrWhiteSpace(rez.Description) && isReturnDescription ? rez.Description : Book.Book.Annot;
                Book.Publisher.ISBN = !string.IsNullOrWhiteSpace(rez.ISBN) && isReturnISBN ? rez.ISBN : Book.Publisher.ISBN;
                Book.Publisher.Pages = rez.PageCount.HasValue && isReturnPageCount ? rez.PageCount : Book.Publisher.Pages;
                Book.Book.Tags = Book.Book.Tags + (!string.IsNullOrWhiteSpace(rez.Categories) && isReturnCategories ? rez.Categories : string.Empty);
                if (isReturnImage && rez.ImageLinks.Scheme != Uri.UriSchemeFile)
                {

                    using (WebClient client = new WebClient())
                    {
                        Book.Book.Cover.BinaryData = client.DownloadData(rez.ImageLinks);
                    }
                }
            }
        }
    }
}

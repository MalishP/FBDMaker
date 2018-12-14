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

namespace FBDMaker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FieldFromText : Window
    {

        private string textFromFile;
        private FBDInf BookInfo;
        public bool isNameFirst { get; set; } = false;
        public bool isOriginalLang { get; set; } = false;
        public string TextFromFile { get => textFromFile; set => textFromFile = value; }

        public FieldFromText(FBDInf BI, string TFF)
        {
            TextFromFile = TFF;
            BookInfo = BI;
            InitializeComponent();
            this.TextBook.Text = TFF;
        }

        private void b_Title_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
                if (isOriginalLang)
                    BookInfo.Book.TitleScr += TextBook.SelectedText;
                else
                {
                    BookInfo.Book.Title += TextBook.SelectedText;
                    BookInfo.Publisher.Title += TextBook.SelectedText;
                }
        }

        private void b_Annot_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
                BookInfo.Book.Annot += TextBook.SelectedText;
        }

        private void b_ISBN_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
                BookInfo.Publisher.ISBN += TextBook.SelectedText;
        }

        private void b_Avtor_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
            {
                string selText = TextBook.SelectedText;
                BookInfo.Book.Avtor.ParseString(selText);
                
            }
        }

        private void b_BookDate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
                BookInfo.Book.s_BookDate = TextBook.SelectedText;
        }

        private void b_Publisher_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
                BookInfo.Publisher.Publisher = TextBook.SelectedText;
        }

        private void b_City_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
                BookInfo.Publisher.City = TextBook.SelectedText;
        }

        private void b_Year_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
            {
                int y;
                if (int.TryParse(TextBook.SelectedText, out y))
                    BookInfo.Publisher.Year = y;
            }
        }

        private void b_Translator_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
            {
                string selText = TextBook.SelectedText;
                BookInfo.Publisher.Translator.ParseString(selText);                
            }
        }

        private void b_UDK_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
                BookInfo.Publisher.UDK= TextBook.SelectedText;
        }

        private void b_BBK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
                BookInfo.Publisher.BBK = TextBook.SelectedText;
        }

        private void b_GRNTI_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
                BookInfo.Publisher.GRNTI = TextBook.SelectedText;
        }

        private void b_Pages_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBook.SelectedText))
            {
                int y;
                if (int.TryParse(TextBook.SelectedText, out y))
                    BookInfo.Publisher.Pages = y;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.IO;
using System.Globalization;
using System.Xml.Linq;
using System.Xml;
using FB2Library;
using FB2Library.Elements;
using FB2Library.HeaderItems;

namespace FBDMaker
{
    public class LibPublisher : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public LibPublisher()
        {
            _Title = new LibLang();
            Translator = new PersonList();
            Painter = new PersonList();
            PublishSequence = new SequenceList();
            Publisher = string.Empty;
            City = string.Empty;
            Year = null;
            Pages = null;
            ISBN = string.Empty;
            UDK = string.Empty;
            BBK = string.Empty;
            GRNTI = string.Empty;
        }
        public LibLang _Title { get; private set; }
        public string City { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public PersonList Translator { get; private set; }
        public PersonList Painter { get; private set; }
        public SequenceList PublishSequence { get; private set; }
        public int? Pages { get; set; }
        public string UDK { get; set; }
        public string BBK { get; set; }
        public string GRNTI { get; set; }

        public string Title
        {
            get { return _Title.BookLang; }
            set { _Title.BookLang = value; }
        }

        public void FillFromFBD(FB2File Fb2)
        {
            if (Fb2.PublishInfo != null)
            {
                _Title.BookLang = Fb2.PublishInfo.BookName == null ? string.Empty : Fb2.PublishInfo.BookName.ToString();
                City = Fb2.PublishInfo.City == null ? string.Empty : Fb2.PublishInfo.City.ToString();
                Year = Fb2.PublishInfo.Year;
                ISBN = Fb2.PublishInfo.ISBN == null ? string.Empty : Fb2.PublishInfo.ISBN.ToString();
                Publisher = Fb2.PublishInfo.Publisher == null ? string.Empty : Fb2.PublishInfo.Publisher.ToString();
                if (Fb2.PublishInfo.Pages!=null)
                {
                    try
                    {
                        Pages = int.Parse(Fb2.PublishInfo.Pages.Text, NumberStyles.Integer);
                    }
                    catch
                    {
                        Pages = null;
                    };
                }
                UDK = Fb2.PublishInfo.UDK == null ? string.Empty : Fb2.PublishInfo.UDK.ToString();
                BBK = Fb2.PublishInfo.BBK == null ? string.Empty : Fb2.PublishInfo.BBK.ToString();
                GRNTI = Fb2.PublishInfo.GRNTI == null ? string.Empty : Fb2.PublishInfo.GRNTI.ToString();
                PublishSequence.FillFromFBD(Fb2.PublishInfo.Sequences, true);
                NotifyPropertyChanged("Title");
                NotifyPropertyChanged("City");
                NotifyPropertyChanged("Year");
                NotifyPropertyChanged("ISBN");
                NotifyPropertyChanged("Publisher");
                NotifyPropertyChanged("Pages");
                NotifyPropertyChanged("UDK");
                NotifyPropertyChanged("BBK");
                NotifyPropertyChanged("GRNTI");
            }
            if (Fb2.TitleInfo.Translators != null)
            {
                Translator.FillFromFBD(Fb2.TitleInfo.Translators);
            }
            if (Fb2.SourceTitleInfo.Translators != null)
            {
                Painter.FillFromFBD(Fb2.SourceTitleInfo.Translators, false);
            }
             if (Fb2.TitleInfo.Painters != null)
            {
                Painter.FillFromFBD(Fb2.TitleInfo.Painters);
            }
             if (Fb2.SourceTitleInfo.Painters != null)
             {
                 Painter.FillFromFBD(Fb2.SourceTitleInfo.Painters, false);
             }
            
            
        }

        internal void Return2FBD(FB2File FBD)
        {
             if (!string.IsNullOrEmpty(_Title.BookLang))
             {
                 if (FBD.PublishInfo.BookName == null)
                     FBD.PublishInfo.BookName = new TextFieldType();
                 FBD.PublishInfo.BookName.Text = _Title.BookLang;
             }
             if (!string.IsNullOrEmpty(City))
             {
                 if (FBD.PublishInfo.City == null)
                     FBD.PublishInfo.City = new TextFieldType();
                 FBD.PublishInfo.City.Text = City;
             }
             if (!string.IsNullOrEmpty(ISBN))
             {
                 if (FBD.PublishInfo.ISBN == null)
                     FBD.PublishInfo.ISBN = new TextFieldType();
                 FBD.PublishInfo.ISBN.Text = ISBN;
             }
            FBD.PublishInfo.Year= Year ;
            if (!string.IsNullOrEmpty(Publisher))
            {
                if (FBD.PublishInfo.Publisher == null)
                    FBD.PublishInfo.Publisher = new TextFieldType();
                FBD.PublishInfo.Publisher.Text = Publisher;
            }
            if (Pages!=null)
            {
                if (FBD.PublishInfo.Pages == null)
                    FBD.PublishInfo.Pages = new TextFieldType();
                FBD.PublishInfo.Pages.Text = Pages.ToString();
            }
            if (!string.IsNullOrEmpty(UDK))
            {
                if (FBD.PublishInfo.UDK == null)
                    FBD.PublishInfo.UDK = new TextFieldType();
                FBD.PublishInfo.UDK.Text = UDK;
            }
            if (!string.IsNullOrEmpty(BBK))
            {
                if (FBD.PublishInfo.BBK == null)
                    FBD.PublishInfo.BBK = new TextFieldType();
                FBD.PublishInfo.BBK.Text = BBK;
            }
            if (!string.IsNullOrEmpty(GRNTI))
            {
                if (FBD.PublishInfo.GRNTI == null)
                    FBD.PublishInfo.GRNTI = new TextFieldType();
                FBD.PublishInfo.GRNTI.Text = GRNTI;
            }
            Translator.Return2FBD(FBD.TitleInfo.Translators, AuthorType.TranslatorElementName, true);
            Translator.Return2FBD(FBD.SourceTitleInfo.Translators, AuthorType.TranslatorElementName, false);
            PublishSequence.Return2FBD(FBD.PublishInfo.Sequences, true);
            Painter.Return2FBD(FBD.TitleInfo.Painters,AuthorType.PainterElementName, true);
            Painter.Return2FBD(FBD.SourceTitleInfo.Painters,AuthorType.PainterElementName, false);

           
        }

        public void CopyFromINF(LibPublisher Source)
        {
            _Title.BookLang = string.IsNullOrEmpty(Source.Title) ? _Title.BookLang : Source.Title;
            _Title.ScrLang = string.IsNullOrEmpty(Source._Title.ScrLang) ? _Title.ScrLang : Source._Title.ScrLang;
            City = string.IsNullOrEmpty(Source.City) ? City : Source.City;
            Year = Source.Year == null ? Year : Source.Year;
            ISBN = string.IsNullOrEmpty(Source.ISBN) ? ISBN : Source.ISBN;
            Publisher = string.IsNullOrEmpty(Source.Publisher) ? Publisher : Source.Publisher;
            Pages = Source.Pages == null ? Pages : Source.Pages;

            UDK = string.IsNullOrEmpty(Source.UDK) ? UDK : Source.UDK;
            BBK = string.IsNullOrEmpty(Source.BBK) ? BBK : Source.BBK;
            GRNTI = string.IsNullOrEmpty(Source.GRNTI) ? GRNTI : Source.GRNTI;
            PublishSequence.CopyFromINF(Source.PublishSequence);

            Translator.CopyFromINF(Source.Translator);
            Painter.CopyFromINF(Source.Painter);
        }
    
    }

}

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
    public class LibFileInf : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public LibFileInf(string fName)
        {
            Creator = new PersonList();
            Publisher = new PersonList();
            ProgUsed = string.Empty;
            DateCreate = null;
            Id = string.Empty;
            Version = string.Empty;
            SCR_URL = new ObservableCollection<string>();
            SCR_OSR = string.Empty;
            History = string.Empty;
            Content = new ObservableCollection<LibContentInf>();
            if (fName != "" && fName != string.Empty)
            {
                Content.Add(new LibContentInf(fName));
            }
        }
        /*     <author> - 1..n (любое число, один обязaтелен);
                <program-used> - 0..1 (один, опционально);
                <date> - 1 (один, обязателен);
                <src-url> - 0..n (любое число, опционально);
                <src-ocr> - 0..1 (один, опционально);
                <id> - 1 (один, обязателен);
                <version> - 1 (один, обязателен);
                <history> - 0..1 (один, опционально);
                <publisher> - 0..n (любое число, опционально) с версии 2.2. 
         */
        public PersonList Creator { get; private set; }
        public PersonList Publisher { get; private set; }
        public string ProgUsed { get; set; }
        public DateTime? DateCreate { get; set; }
        public string Id { get; set; }
        public string Version { get; set; }
        public ObservableCollection<string> SCR_URL { get; set; }
        public string SCR_OSR { get; set; }
        public string History { get; set; }
        public ObservableCollection<LibContentInf> Content { get; private set; }

        internal void FillFromFBD(FB2File FBD)
        {
            ItemDocumentInfo Fdoc = FBD.DocumentInfo;
            Creator.FillFromFBD(Fdoc.DocumentAuthors);
            Publisher.FillFromFBD(Fdoc.DocumentPublishers);
            ProgUsed = Fdoc.ProgramUsed2Create == null ? String.Empty : Fdoc.ProgramUsed2Create.ToString();

            DateCreate = Fdoc.DocumentDate == null ? (DateTime?)null : Fdoc.DocumentDate.DateValue;
            Id = Fdoc.ID;
            Version = Fdoc.DocumentVersion == null ? string.Empty : Fdoc.DocumentVersion.ToString();
            foreach (string s in Fdoc.SourceURLs)
            {
                SCR_URL.Add(s);
            }
            SCR_OSR = Fdoc.SourceOCR == null ? string.Empty : Fdoc.SourceOCR.ToString();
            History = Fdoc.History == null ? string.Empty : Fdoc.History.ToString();
            NotifyPropertyChanged("ProgUsed");
            NotifyPropertyChanged("DateCreate");
            NotifyPropertyChanged("Id");
            NotifyPropertyChanged("Version");
            NotifyPropertyChanged("SCR_OSR");
            NotifyPropertyChanged("History");
        }
        internal void Return2FBD(FB2File FBD)
        {
            ItemDocumentInfo Fdoc = FBD.DocumentInfo;
           
            Creator.Return2FBD(Fdoc.DocumentAuthors,AuthorType.AuthorElementName);
            Publisher.Return2FBD(Fdoc.DocumentPublishers,AuthorType.PublisherElementName);
            if (!string.IsNullOrEmpty(ProgUsed))
            {
                if (Fdoc.ProgramUsed2Create == null)
                    Fdoc.ProgramUsed2Create = new TextFieldType();
                Fdoc.ProgramUsed2Create.Text = ProgUsed;
            }
            if (DateCreate!=null)
            {
                if (Fdoc.DocumentDate == null)
                    Fdoc.DocumentDate = new DateItem();
                Fdoc.DocumentDate.DateValue =(DateTime) DateCreate;
            }

            Fdoc.ID=Id;
            if (!string.IsNullOrEmpty(Version))
            {
                try{
                    Fdoc.DocumentVersion = float.Parse(Version, NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite);
                }
                catch
                {
                    Fdoc.DocumentVersion = null;
                }
            }

            Fdoc.SourceURLs.Clear();
            foreach (string s in SCR_URL)
            {
                Fdoc.SourceURLs.Add(s);
            }
            if (!string.IsNullOrEmpty(SCR_OSR))
            {
                if (Fdoc.SourceOCR == null)
                    Fdoc.SourceOCR = new TextFieldType();
                Fdoc.SourceOCR.Text = SCR_OSR;
            }

            if (!string.IsNullOrEmpty(History))
            {
                if (Fdoc.History == null)
                    Fdoc.History = new AnnotationType();
                Fdoc.History.Content.Clear();
                string[] Separators = new string[] { "\n" };
                foreach (string s in History.Split(Separators, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrEmpty(s.TrimStart(' ', '\t')))
                    {
                        ParagraphItem paragraph = new ParagraphItem();
                        SimpleText text = new SimpleText();
                        text.Style = TextStyles.Normal;
                        text.Text = s;
                        paragraph.ParagraphData.Add(text);
                        Fdoc.History.Content.Add(paragraph);
                    }
                }
               
            }
            
        }

    }



    public class LibContentInf : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public LibContentInf(string In_FileName = "")
        {
            _FileName = (In_FileName == "" ? "Текущий" : In_FileName);
            _Coment = string.Empty;
            _OSR = false;
            _Quality = 3;
        }


        private string _FileName;
        private Boolean _OSR;
        private string _Coment;
        private int _Quality;

        public string FileName
        {
            get { return _FileName; }
            set
            {
                _FileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public string Coment
        {
            get { return _Coment; }
            set
            {
                _Coment = value;
                NotifyPropertyChanged("Coment");
            }
        }

        public Boolean OSR
        {
            get { return _OSR; }
            set
            {
                _OSR = value;
                NotifyPropertyChanged("OSR");
            }
        }

        public int Quality
        {
            get { return _Quality; }
            set
            {
                _Quality = value;
                NotifyPropertyChanged("Quality");
            }
        }
    }
}

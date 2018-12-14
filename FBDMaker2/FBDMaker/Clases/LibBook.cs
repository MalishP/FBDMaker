using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FB2Library;
using FB2Library.Elements;
using FB2Library.HeaderItems;
//using System.Globalization;
//using System.Xml.Linq;
//using System.Xml;
//using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
//using System.Windows.Media.Imaging;
//using System.IO;

namespace FBDMaker
{
    public class LibBook : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public LibBook()
        {
            Avtor = new PersonList();
            _Title = new LibLang();
            _Theme = new GenreList();
            _Annot = new LibAnnot();
            _Tag = new ObservableCollection<string>();
            d_BookDate = null;
            s_BookDate = string.Empty;
            Cover = new LibCover();
            _Lang = new LibLang();
            BookSequence = new SequenceList(); //new ObservableCollection<LibSequence>();
        }

      //  private ItemTitleInfo _FTitle, _STitle;
        public PersonList Avtor { get; private set; }
        //public ObservableCollection<LibPerson> Avtor { get; private set; }
        //public List<LibPerson> Avtor { get; private set; }
        public LibLang _Title { get; private set; }
        public GenreList _Theme { get; private set; }
        public LibAnnot _Annot { get; private set; }
        public ObservableCollection<string> _Tag { get; private set; }
       
        private  DateTime? _d_BookDate;
        public DateTime? d_BookDate {
            get { return _d_BookDate; }
            set
            {
                _d_BookDate = value;
                NotifyPropertyChanged("d_BookDate");

                if (value != null)
                {
                    if (string.IsNullOrEmpty(_s_BookDate))
                    {
                        DateTime tmp=(DateTime)value;
                        s_BookDate=tmp.ToShortDateString();
                    }
                    //if (_FTitle.BookDate == null)
                    //{
                    //    _FTitle.BookDate = new DateItem();
                    //    _FTitle.BookDate.Text = string.Empty;
                    //    _FTitle.BookDate.DateValue = DateTime.MinValue;
                    //}
                    //_FTitle.BookDate.DateValue = (DateTime)value; //value == null ? DateTime.MinValue :
                    //if (_FTitle.BookDate.Text == string.Empty)
                    //{
                    //    _FTitle.BookDate.Text = _FTitle.BookDate.DateValue.ToShortDateString();
                    //}

                    //if (_STitle.BookDate == null)
                    //{
                    //    _STitle.BookDate = new DateItem();
                    //    _STitle.BookDate.Text = string.Empty;
                    //    _STitle.BookDate.DateValue = DateTime.MinValue;
                    //}
                    //_STitle.BookDate.DateValue = (DateTime)value; //value == null ? DateTime.MinValue :
                    //if (_STitle.BookDate.Text == string.Empty)
                    //{
                    //    _STitle.BookDate.Text = _STitle.BookDate.DateValue.ToShortDateString();
                    //}

                }
            } 
        }

        private string _s_BookDate;
        public string s_BookDate
        {
            get { return _s_BookDate; }
            set {
                _s_BookDate = value;
                NotifyPropertyChanged("s_BookDate");

                //if (!string.IsNullOrEmpty(value))
                //{
                //    if (_FTitle.BookDate == null)
                //    {
                //        _FTitle.BookDate = new DateItem();
                //        _FTitle.BookDate.Text=string.Empty;
                //        _FTitle.BookDate.DateValue=DateTime.MinValue;
                //    }
                        
                //        _FTitle.BookDate.Text = value;
                //    if (_FTitle.BookDate.DateValue.Equals(DateTime.MinValue))
                //    {
                //        try
                //        {
                //            _FTitle.BookDate.DateValue = DateTime.Parse(value);
                //        }
                //        catch (Exception)
                //        {
                //            _FTitle.BookDate.DateValue = DateTime.MinValue;
                //        }
                //    }

                //     if (_STitle.BookDate == null)
                //    {
                //        _STitle.BookDate = new DateItem();
                //        _STitle.BookDate.Text=string.Empty;
                //        _STitle.BookDate.DateValue=DateTime.MinValue;
                //    }
                        
                //        _STitle.BookDate.Text = value;
                //    if (_STitle.BookDate.DateValue.Equals(DateTime.MinValue))
                //    {
                //        try
                //        {
                //            _STitle.BookDate.DateValue = DateTime.Parse(value);
                //        }
                //        catch (Exception)
                //        {
                //            _STitle.BookDate.DateValue = DateTime.MinValue;
                //        }
                //    }
                //}
            }
        }
        public LibCover Cover { get; private set; }
        public LibLang _Lang { get; private set; }
        public SequenceList BookSequence { get; private set; }
        //public ObservableCollection<LibSequence> BookSequence { get; private set; }

        

        public string Annot
        {
            get { return _Annot.Text; }
            set
            {
                _Annot.Text = value;
                NotifyPropertyChanged("Annot");
            }
        }
        public string Title
        {
            get { return _Title.BookLang; }
            set
            {
                _Title.BookLang = value;
                NotifyPropertyChanged("Title");
                //if (_FTitle.BookTitle == null)
                //    _FTitle.BookTitle = new TextFieldType();

                //_FTitle.BookTitle.Text = value;
            }
        }
        public string Lang
        {
            get { return _Lang.BookLang; }
            set { _Lang.BookLang = value;
                NotifyPropertyChanged("Lang");

             //   _FTitle.Language = value;
            }
        }
        public string TitleScr
        {
            get { return _Title.ScrLang; }
            set { _Title.ScrLang = value;
            //if (_STitle.BookTitle == null)
            //    _STitle.BookTitle = new TextFieldType();

            //_STitle.BookTitle.Text = value;
            }
        }
        public string LangScr
        {
            get { return _Lang.ScrLang; }
            set { _Lang.ScrLang = value;
            NotifyPropertyChanged("LangScr");
         //   _FTitle.SrcLanguage = value;
            }
        }
        public string Tags
        {
            get
            {
                string sRet = "";
                foreach (string s in _Tag)
                    sRet += (s + "; ");
                return sRet;
            }
            set
            {
                string sval = value;
                string[] Separators = new string[] { "; ", ";", " ;" };
                _Tag.Clear();
                foreach (string s in sval.Split(Separators, System.StringSplitOptions.RemoveEmptyEntries))
                    _Tag.Add(s);
                //if (_FTitle.Keywords == null)
                //    _FTitle.Keywords = new TextFieldType();
                //_FTitle.Keywords.Text = value;
            }
        }
        public string Themes
        {
            get
            {
                return _Theme.SelectedNameList;
            }
        }

        public string FileName2Rename
        {
        get{
            string retstr=string.Empty;
            foreach(LibPerson a in Avtor.List)
            {
                retstr+=", "+a.ListName;
            }
            retstr=retstr.Substring(2)+" - "+Title;
            return retstr;
            }
        }

        internal void FillFromFBD(FB2File FBD)
        {

            ItemTitleInfo FTitle = FBD.TitleInfo;
       //     _FTitle = FTitle;
            ItemTitleInfo STitle = FBD.SourceTitleInfo;
      //      _STitle = STitle;

            Title = FTitle.BookTitle==null ? string.Empty: FTitle.BookTitle.ToString();
            Annot = FTitle.Annotation==null ? string.Empty: FTitle.Annotation.ToString();
            d_BookDate = FTitle.BookDate == null || FTitle.BookDate.DateValue == null ? (DateTime?)null : FTitle.BookDate.DateValue;
            s_BookDate = FTitle.BookDate == null || string.IsNullOrEmpty(FTitle.BookDate.Text) ? string.Empty : FTitle.BookDate.Text;
            Lang = string.IsNullOrEmpty(FTitle.Language) ? string.Empty : FTitle.Language;
            LangScr = string.IsNullOrEmpty(FTitle.SrcLanguage) ? string.Empty : FTitle.SrcLanguage;
            //-------------------------------------
           
            TitleScr = STitle.BookTitle == null ? string.Empty : STitle.BookTitle.ToString();
            //***********************************************
            #region Заполнение Списка авторов
            Avtor.FillFromFBD(FTitle.BookAuthors, true);
            Avtor.FillFromFBD(FBD.SourceTitleInfo.BookAuthors, false);
            #endregion

            #region Список жанров и тегов
            foreach (TitleGenreType tgt in FTitle.Genres)
            {
                _Theme.CheckGenre(tgt.Genre);
            }
            string sval = FTitle.Keywords == null ? string.Empty : FTitle.Keywords.ToString();
            if (!string.IsNullOrEmpty(sval))
            {
                string[] Separators = new string[] { "; ", ";", " ;" };
                _Tag.Clear();
                foreach (string s in sval.Split(Separators, System.StringSplitOptions.RemoveEmptyEntries))
                    _Tag.Add(s);
            }
            #endregion

            #region Заполнение обложки
            Cover.FillFromFBD(FBD);

            #endregion

            #region Заполнение серий
            BookSequence.FillFromFBD(FTitle.Sequences,true);
            BookSequence.FillFromFBD(FBD.SourceTitleInfo.Sequences, false);
            #endregion

        }
        internal void Return2FBD(FB2File FBD)
        {

            ItemTitleInfo FTitle = FBD.TitleInfo;
       //     _FTitle = FTitle;
            if (FTitle.BookTitle == null)
                FTitle.BookTitle = new TextFieldType();
            FTitle.BookTitle.Text = string.IsNullOrEmpty(Title) ? string.Empty : Title;
            if(!string.IsNullOrEmpty(s_BookDate) || d_BookDate!=null)
            {
                if (FTitle.BookDate == null)
                {
                    FTitle.BookDate = new DateItem();
                }
                FTitle.BookDate.Text = s_BookDate;
                FTitle.BookDate.DateValue = d_BookDate==null?DateTime.MinValue:(DateTime)d_BookDate ;
            }
            if (!string.IsNullOrEmpty(Lang))
            {
                FTitle.Language = Lang;
            }
            if (!string.IsNullOrEmpty(LangScr))
            {
                FTitle.SrcLanguage = LangScr;
            }
            if (!string.IsNullOrEmpty(Annot))
            {
                if (FTitle.Annotation == null)
                    FTitle.Annotation = new AnnotationItem();
                string[] Separators = new string[] { "\n" };
                FTitle.Annotation.Content.Clear();
                foreach (string s in Annot.Split(Separators, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    if(!string.IsNullOrEmpty(s.TrimStart(' ','\t')))
                    {
                        ParagraphItem paragraph = new ParagraphItem();
                         SimpleText text = new SimpleText();
                        text.Style=TextStyles.Normal;
                        text.Text=s;
                        paragraph.ParagraphData.Add(text);
                        FTitle.Annotation.Content.Add(paragraph);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Tags))
            {
                if (FTitle.Keywords == null)
                    FTitle.Keywords = new TextFieldType();
                FTitle.Keywords.Text = Tags;
            }
            Avtor.Return2FBD(FTitle.BookAuthors,AuthorType.AuthorElementName, true);
            BookSequence.Return2FBD(FTitle.Sequences, true);
            Cover.Return2FBD(FBD);

            List<string> tmpGen=_Theme.GetCheckedGenreList();
            FTitle.Genres.Clear();
            foreach (string s in tmpGen)
            {
                TitleGenreType genre = new TitleGenreType();
                 genre.Genre = s;
                 FTitle.Genres.Add(genre);
            }

            ItemTitleInfo STitle = FBD.SourceTitleInfo;
            //      _STitle = STitle;
            if (!string.IsNullOrEmpty(TitleScr))
            {
                if (STitle.BookTitle == null)
                    STitle.BookTitle = new TextFieldType();
                STitle.BookTitle.Text = string.IsNullOrEmpty(TitleScr) ? string.Empty : TitleScr;
            }
            Avtor.Return2FBD(STitle.BookAuthors, AuthorType.AuthorElementName, false);
            BookSequence.Return2FBD(STitle.Sequences, false);


        }


        internal void FillFromFileMask(StringMaskParser Mask)
        {
            Dictionary<string, string> Dict = Mask.Dict;//StringMaskParser.Parse(PName, Mask);
            List<string> keys = Dict.Keys.ToList<string>();
            foreach (string s in keys.Where(s => !(string.IsNullOrEmpty(Dict[s]) || string.IsNullOrWhiteSpace(Dict[s]))))
            {
                string num_seq;
                string mFirst, mMidl, mNick;
                int? pYear;
                switch (s)
                {
                    case "tl":
                        Title = Dict[s];
                        break;
                    case "st":
                        num_seq = (!(string.IsNullOrEmpty(Dict["sn"]) || string.IsNullOrWhiteSpace(Dict["sn"]))) ? Dict["sn"] : string.Empty;
                        BookSequence.List.Add(new LibSequence(null, Dict[s], num_seq));
                        break;
                    case "av":
                        Avtor.ParseString(Dict["av"]);
                        break;
                    case "nl1":
                        mFirst = (!(string.IsNullOrEmpty(Dict["nf1"]) || string.IsNullOrWhiteSpace(Dict["nf1"]))) ? Dict["nf1"] : string.Empty;
                        mMidl = (!(string.IsNullOrEmpty(Dict["nm1"]) || string.IsNullOrWhiteSpace(Dict["nm1"]))) ? Dict["nm1"] : string.Empty;
                        mNick = (!(string.IsNullOrEmpty(Dict["nn1"]) || string.IsNullOrWhiteSpace(Dict["nn1"]))) ? Dict["nn1"] : string.Empty;
                        Avtor.List.Add(new LibPerson(Dict[s], mFirst, mMidl, mNick));
                        break;
                    case "nl2":
                        mFirst = (!(string.IsNullOrEmpty(Dict["nf2"]) || string.IsNullOrWhiteSpace(Dict["nf2"]))) ? Dict["nf2"] : string.Empty;
                        mMidl = (!(string.IsNullOrEmpty(Dict["nm2"]) || string.IsNullOrWhiteSpace(Dict["nm2"]))) ? Dict["nm2"] : string.Empty;
                        mNick = (!(string.IsNullOrEmpty(Dict["nn2"]) || string.IsNullOrWhiteSpace(Dict["nn2"]))) ? Dict["nn2"] : string.Empty;
                        Avtor.List.Add(new LibPerson(Dict[s], mFirst, mMidl, mNick));
                        break;
                    case "nl3":
                        mFirst = (!(string.IsNullOrEmpty(Dict["nf3"]) || string.IsNullOrWhiteSpace(Dict["nf3"]))) ? Dict["nf3"] : string.Empty;
                        mMidl = (!(string.IsNullOrEmpty(Dict["nm3"]) || string.IsNullOrWhiteSpace(Dict["nm3"]))) ? Dict["nm3"] : string.Empty;
                        mNick = (!(string.IsNullOrEmpty(Dict["nn3"]) || string.IsNullOrWhiteSpace(Dict["nn3"]))) ? Dict["nn3"] : string.Empty;
                        Avtor.List.Add(new LibPerson(Dict[s], mFirst, mMidl, mNick));
                        break;
                    case "nl4":
                        mFirst = (!(string.IsNullOrEmpty(Dict["nf4"]) || string.IsNullOrWhiteSpace(Dict["nf4"]))) ? Dict["nf4"] : string.Empty;
                        mMidl = (!(string.IsNullOrEmpty(Dict["nm4"]) || string.IsNullOrWhiteSpace(Dict["nm4"]))) ? Dict["nm4"] : string.Empty;
                        mNick = (!(string.IsNullOrEmpty(Dict["nn4"]) || string.IsNullOrWhiteSpace(Dict["nn4"]))) ? Dict["nn4"] : string.Empty;
                        Avtor.List.Add(new LibPerson(Dict[s], mFirst, mMidl, mNick));
                        break;
                    case "nl5":
                        mFirst = (!(string.IsNullOrEmpty(Dict["nf5"]) || string.IsNullOrWhiteSpace(Dict["nf5"]))) ? Dict["nf5"] : string.Empty;
                        mMidl = (!(string.IsNullOrEmpty(Dict["nm5"]) || string.IsNullOrWhiteSpace(Dict["nm5"]))) ? Dict["nm5"] : string.Empty;
                        mNick = (!(string.IsNullOrEmpty(Dict["nn5"]) || string.IsNullOrWhiteSpace(Dict["nn5"]))) ? Dict["nn5"] : string.Empty;
                        Avtor.List.Add(new LibPerson(Dict[s], mFirst, mMidl, mNick));
                        break;
                    //case "ed":
                    //    Publisher.Publisher = Dict[s];
                    //    break;
                    //case "ct":
                    //    Publisher.City = Dict[s];
                    //    break;
                    //case "ye":
                    //    try { pYear = int.Parse(Dict[s], NumberStyles.Integer); }
                    //    catch { pYear = null; };
                    //    Publisher.Year = pYear;
                    //    break;
                }
            }
            // throw new NotImplementedException();
        }

        public void CopyFromINF(LibBook Source)
        {
            Title = !string.IsNullOrEmpty(Source.Title) ? Source.Title + "(" + Title + ")" : Title;
            Annot = !string.IsNullOrEmpty(Source.Annot) ? Source.Annot : Annot;
            _d_BookDate = Source._d_BookDate == null ? _d_BookDate : Source._d_BookDate;
            _s_BookDate = string.IsNullOrEmpty(Source._s_BookDate) ? _s_BookDate : Source._s_BookDate;

            Lang = string.IsNullOrEmpty(Source.Lang) ? Lang : Source.Lang;
            LangScr = string.IsNullOrEmpty(Source.LangScr) ? LangScr : Source.LangScr;
                        //-------------------------------------

            TitleScr = !string.IsNullOrEmpty(Source.TitleScr) ? Source.TitleScr : TitleScr;
            //***********************************************
            #region Заполнение Списка авторов
            Avtor.CopyFromINF(Source.Avtor);
            #endregion

            #region Список жанров и тегов
            foreach (string s in Source._Theme.GetCheckedGenreList())
            {
                _Theme.CheckGenre(s);
            }
            foreach(string s in Source._Tag)
            {
                if (!_Tag.Where(st => st == s).Any())
                {
                    _Tag.Add(s);
                }
            }
            #endregion

            #region Заполнение обложки

            if (!string.IsNullOrEmpty(Source.Cover.CoverName))
                Cover = Source.Cover;
            #endregion

            #region Заполнение серий
            BookSequence.CopyFromINF(Source.BookSequence);
            #endregion
        }

    }

   
}

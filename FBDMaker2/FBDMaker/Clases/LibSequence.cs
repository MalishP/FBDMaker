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

    public class SequenceList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public SequenceList()
        {
            List = new ObservableCollection<LibSequence>();
        }


        public ObservableCollection<LibSequence> List { get; private set; }
        // private 
        public void FillFromFBD(IEnumerable<SequenceType> BookSequence, Boolean is_not_source = true)
        {
            int id2Seq = 1;
            if (is_not_source)
            {
                foreach (SequenceType fbd_Sequence in BookSequence)
                {
                    LibSequence new_seq = new LibSequence(null, fbd_Sequence.Name, fbd_Sequence.Number);
                    if (!string.IsNullOrEmpty(fbd_Sequence.UID))
                        new_seq.UID = fbd_Sequence.UID;
                    else
                        new_seq.UID = id2Seq.ToString();

                    if (!string.IsNullOrEmpty(fbd_Sequence.sNumber))
                        new_seq.s_Num = fbd_Sequence.sNumber;
                    id2Seq = new_seq.FillFromFBD(fbd_Sequence.SubSections, id2Seq, is_not_source);
                    List.Add(new_seq);
                }
            }
            else
            {
                foreach (SequenceType fbd_Sequence in BookSequence)
                {
                    string tmp_UID;
                    if (!string.IsNullOrEmpty(fbd_Sequence.UID))
                        tmp_UID = fbd_Sequence.UID;
                    else
                        tmp_UID=id2Seq.ToString();

                    LibSequence tmp_seq = List.First(s => string.Equals(s.UID, tmp_UID));
                    id2Seq++;
                    if (tmp_seq != null)
                    {
                        tmp_seq.NameScr = fbd_Sequence.Name;
                        id2Seq = tmp_seq.FillFromFBD(fbd_Sequence.SubSections, id2Seq, is_not_source);
                    }
                    

                }
            }
            NotifyPropertyChanged("List");

        }

        public void CopyFromINF (SequenceList Source)
        {
            foreach (LibSequence s in Source.List)
            {
                if(!List.Where(sec=>sec.Name==s.Name).Any())
                {
                    List.Add(s);
                }
            }
        }

        public void Return2FBD (List<SequenceType> BookSequence, Boolean is_not_source = true)
        {
            BookSequence.Clear();

            if (is_not_source)
            {
                foreach (LibSequence fbd_Sequence in List)
                {

                    SequenceType new_seq = new SequenceType();
                    new_seq.Name = fbd_Sequence.Name;
                    new_seq.Number = fbd_Sequence.n_Num;
                    new_seq.sNumber = fbd_Sequence.s_Num;
                    new_seq.UID = fbd_Sequence.UID;
                    fbd_Sequence.Return2FBD(new_seq.SubSections, is_not_source);
                    BookSequence.Add(new_seq);

                }
            }
            else
            {
                foreach (LibSequence fbd_Sequence in List.Where(s=>!string.IsNullOrEmpty(s.NameScr)))
                {

                    SequenceType new_seq = new SequenceType();
                    new_seq.Name = fbd_Sequence.NameScr;
                    new_seq.Number = fbd_Sequence.n_Num;
                    new_seq.sNumber = fbd_Sequence.s_Num;
                    new_seq.UID = fbd_Sequence.UID;
                    fbd_Sequence.Return2FBD(new_seq.SubSections, is_not_source);
                    BookSequence.Add(new_seq);


                }
            }

        }

    }
    public class LibSequence : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public LibSequence(LibSequence parent, string name = "", int? num = null)
        {
            _Name = new LibLang(name);
            _n_Num = num;
            SubSequence = new ObservableCollection<LibSequence>();
            Parent = parent;
            _s_Num = (_n_Num == null ? string.Empty : _n_Num.ToString());
            UID = string.Empty;
        }

        public LibSequence(LibSequence parent, string name, string num)
        {
            _Name = new LibLang(name);
            _s_Num = num;
            SubSequence = new ObservableCollection<LibSequence>();
            Parent = parent;
            try
            {
                _n_Num = int.Parse(_s_Num, NumberStyles.Integer);
            }
            catch
            {
                _n_Num = null;
            };
            UID = string.Empty;
        }
        public LibSequence Parent { get; private set; }
        public LibLang _Name { get; private set; }
        public int? _n_Num { get; set; }
        public string _s_Num { get; set; }
        public string UID { get; set; }
        public ObservableCollection<LibSequence> SubSequence { get; private set; }
        public string Name
        {
            get { return (_Name.BookLang == string.Empty ? "Empty" : _Name.BookLang); }
            set
            {
                _Name.BookLang = value;
                NotifyPropertyChanged("Name");
            }
        }
        public string NameScr
        {
            get { return _Name.ScrLang; }
            set
            {
                _Name.ScrLang = value;
                NotifyPropertyChanged("NameScr");
            }
        }
        public string s_Num
        {
            get { return _s_Num; }
            set
            {
                _s_Num = value;
                NotifyPropertyChanged("s_Num");
                if (_n_Num == null)
                {
                    try
                    {
                        _n_Num = int.Parse(_s_Num, NumberStyles.Integer);
                    }
                    catch
                    {
                        _n_Num = null;
                    };
                    NotifyPropertyChanged("n_Num");
                }
            }
        }
        public int? n_Num
        {
            get { return _n_Num; }
            set
            {
                _n_Num = value;
                if (string.IsNullOrEmpty(_s_Num))
                {
                    _s_Num = (_n_Num == null ? string.Empty : _n_Num.ToString());
                    NotifyPropertyChanged("s_Num");
                }
                NotifyPropertyChanged("n_Num");
            }
        }

        //public void LoadFromFb2(SequenceType seq, Dictionary<int, LibSequence> dict2seq, ref int id2Seq)
        //{
        //    foreach (SequenceType fbd_seq in seq.SubSections)
        //    {
        //        LibSequence new_seq = new LibSequence(this, fbd_seq.Name, fbd_seq.Number);
        //        dict2seq.Add(id2Seq, new_seq);
        //        id2Seq++;
        //        new_seq.LoadFromFb2(fbd_seq, dict2seq, ref id2Seq);
        //        SubSequence.Add(new_seq);

        //    }
        //}

        //public void LoadScrFromFb2(SequenceType seq, Dictionary<int, LibSequence> dict2seq, ref int id2Seq)
        //{
        //    foreach (SequenceType fbd_seq in seq.SubSections)
        //    {
        //        dict2seq[id2Seq].NameScr = fbd_seq.Name;
        //        id2Seq++;
        //        dict2seq[id2Seq].LoadScrFromFb2(fbd_seq, dict2seq, ref id2Seq);
        //    }
        //}

        public int FillFromFBD(IEnumerable<SequenceType> BookSequence, int id2Seq, Boolean is_not_source = true)
        {
            if (is_not_source)
            {
                foreach (SequenceType fbd_Sequence in BookSequence)
                {
                    LibSequence new_seq = new LibSequence(this, fbd_Sequence.Name, fbd_Sequence.Number);
                    if (!string.IsNullOrEmpty(fbd_Sequence.UID))
                        new_seq.UID = fbd_Sequence.UID;
                    else
                        new_seq.UID = id2Seq.ToString();

                    if (!string.IsNullOrEmpty(fbd_Sequence.sNumber))
                        new_seq.s_Num = fbd_Sequence.sNumber;
                    id2Seq++;
                    id2Seq = new_seq.FillFromFBD(fbd_Sequence.SubSections,id2Seq, is_not_source);
                    SubSequence.Add(new_seq);
                }
            }
            else
            {
                foreach (SequenceType fbd_Sequence in BookSequence)
                {
                     string tmp_UID;
                    if (!string.IsNullOrEmpty(fbd_Sequence.UID))
                        tmp_UID = fbd_Sequence.UID;
                    else
                        tmp_UID = id2Seq.ToString();
                    LibSequence tmp_seq = SubSequence.First(s => string.Equals(s.UID, tmp_UID));
                    id2Seq++;
                    if (tmp_seq != null)
                    {
                        tmp_seq.NameScr = fbd_Sequence.Name ;
                        id2Seq = tmp_seq.FillFromFBD(fbd_Sequence.SubSections, id2Seq, is_not_source);                     
                    }
                    

                }
            }
            return id2Seq;
        }

        public void Return2FBD (List<SequenceType> BookSequence, Boolean is_not_source = true)
        {
            BookSequence.Clear();

            if (is_not_source)
            {
                foreach (LibSequence fbd_Sequence in SubSequence)
                {

                    SequenceType new_seq = new SequenceType();
                    new_seq.Name = fbd_Sequence.Name;
                    new_seq.Number = fbd_Sequence.n_Num;
                    new_seq.sNumber = fbd_Sequence.s_Num;
                    new_seq.UID = fbd_Sequence.UID;
                    fbd_Sequence.Return2FBD(new_seq.SubSections, is_not_source);
                    BookSequence.Add(new_seq);

                }
            }
            else
            {
                foreach (LibSequence fbd_Sequence in SubSequence.Where(s=>!string.IsNullOrEmpty(s.NameScr)))
                {

                    SequenceType new_seq = new SequenceType();
                    new_seq.Name = fbd_Sequence.NameScr;
                    new_seq.Number = fbd_Sequence.n_Num;
                    new_seq.sNumber = fbd_Sequence.s_Num;
                    new_seq.UID = fbd_Sequence.UID;
                    fbd_Sequence.Return2FBD(new_seq.SubSections, is_not_source);
                    BookSequence.Add(new_seq);


                }
            }

        }
    }
}

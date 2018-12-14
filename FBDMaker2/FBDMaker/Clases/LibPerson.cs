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

    public class PersonList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public PersonList()
        {
            List = new ObservableCollection<LibPerson>();
        }
        
        public ObservableCollection<LibPerson> List { get; private set; }
       // private 


        internal void FillFromFBD(IEnumerable<AuthorType> BookAuthors, Boolean is_not_source = true)
        {
            int id2author = 1;
            if (is_not_source)
            {
                foreach (AuthorType fbd_author in BookAuthors)
                {
                    LibPerson new_person = new LibPerson();
                    //new_person.PersonType = "Автор";
                    new_person.PersonType = fbd_author.A_Type == null ? string.Empty : fbd_author.A_Type.ToString();
                    new_person.First = fbd_author.FirstName == null ? string.Empty : fbd_author.FirstName.ToString();
                    new_person.Last = fbd_author.LastName == null ? string.Empty : fbd_author.LastName.ToString();
                    new_person.Middle = fbd_author.MiddleName == null ? string.Empty : fbd_author.MiddleName.ToString();
                    new_person.Nick = fbd_author.NickName == null ? string.Empty : fbd_author.NickName.ToString();
                    new_person.Email = fbd_author.EMail == null ? string.Empty : fbd_author.EMail.ToString();
                    new_person.HomePages = fbd_author.HomePage == null ? string.Empty : fbd_author.HomePage.ToString();

                    if (fbd_author.UID == null || string.IsNullOrEmpty(fbd_author.UID.Text))
                    {
                        new_person.UID = id2author.ToString();
                    }
                    else
                    {
                        new_person.UID = fbd_author.UID.ToString();
                    }
                    id2author++;
                    List.Add(new_person);
                }
            }
            else
            {

                id2author = 1;
                foreach (AuthorType fbd_author in BookAuthors)
                {
                    string tmp_UID = fbd_author.UID == null || string.IsNullOrEmpty(fbd_author.UID.Text) ? id2author.ToString() : fbd_author.UID.ToString();
                    LibPerson tmp_pers = null;
                    try
                    {
                        tmp_pers = List.First(p => string.Equals(p.UID, tmp_UID));
                    }
                    catch { tmp_pers = null; }
                    if (tmp_pers != null)
                    {
                        tmp_pers.FirstScr = fbd_author.FirstName == null ? string.Empty : fbd_author.FirstName.ToString();
                        tmp_pers.LastScr = fbd_author.LastName == null ? string.Empty : fbd_author.LastName.ToString();
                        tmp_pers.MiddleScr = fbd_author.MiddleName == null ? string.Empty : fbd_author.MiddleName.ToString();
                    }
                    id2author++;

                }
            }
        }
         internal void Return2FBD(List <AuthorType> BookAuthors,string ElementName, Boolean is_not_source = true)
        {
            int id2author = 1;
            BookAuthors.Clear();
            if (is_not_source)
            {
             
                foreach (LibPerson fbd_author in List)
                {
                    if (!string.IsNullOrEmpty(fbd_author.First) || !string.IsNullOrEmpty(fbd_author.Nick) || !string.IsNullOrEmpty(fbd_author.Last))
                   {
                        AuthorType new_person = new AuthorType();
                        new_person.ElementName = ElementName;
                     //   new_person.PersonType = "Автор";
                        if(!string.IsNullOrEmpty(fbd_author.PersonType))
                        {
                            new_person.A_Type=new TextFieldType();
                            new_person.A_Type.Text = fbd_author.PersonType;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.First))
                        {
                            new_person.FirstName=new TextFieldType();
                            new_person.FirstName.Text = fbd_author.First;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.Last))
                        {
                            new_person.LastName=new TextFieldType();
                            new_person.LastName.Text = fbd_author.Last;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.Middle))
                        {
                            new_person.MiddleName=new TextFieldType();
                            new_person.MiddleName.Text = fbd_author.Middle;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.Nick))
                        {
                            new_person.NickName=new TextFieldType();
                            new_person.NickName.Text = fbd_author.Nick;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.Email))
                        {
                            new_person.EMail=new TextFieldType();
                            new_person.EMail.Text = fbd_author.Email;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.HomePages))
                        {
                            new_person.HomePage=new TextFieldType();
                            new_person.HomePage.Text = fbd_author.HomePages;
                        }
                         if(!string.IsNullOrEmpty(fbd_author.UID))
                        {
                            new_person.UID=new TextFieldType();
                            new_person.UID.Text = fbd_author.UID;
                        }
                         else
                         {
                            new_person.UID=new TextFieldType();
                            new_person.UID.Text =  id2author.ToString();
                         }
                        id2author++;
                        BookAuthors.Add(new_person);
                   }
                }
            }
            else
            {
                
                 foreach (LibPerson fbd_author in List.Where(s=>!string.IsNullOrEmpty(s.FirstScr)||!string.IsNullOrEmpty(s.LastScr)))
                {
                   if(!string.IsNullOrEmpty(fbd_author.FirstScr) ||!string.IsNullOrEmpty(fbd_author.LastScr)||!string.IsNullOrEmpty(fbd_author.MiddleScr))
                   {
                        AuthorType new_person = new AuthorType();
                        new_person.ElementName = ElementName;
                     //   new_person.PersonType = "Автор";
                        if(!string.IsNullOrEmpty(fbd_author.PersonType))
                        {
                            new_person.A_Type=new TextFieldType();
                            new_person.A_Type.Text = fbd_author.PersonType;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.FirstScr))
                        {
                            new_person.FirstName=new TextFieldType();
                            new_person.FirstName.Text = fbd_author.FirstScr;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.LastScr))
                        {
                            new_person.LastName=new TextFieldType();
                            new_person.LastName.Text = fbd_author.LastScr;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.MiddleScr))
                        {
                            new_person.MiddleName=new TextFieldType();
                            new_person.MiddleName.Text = fbd_author.MiddleScr;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.Nick))
                        {
                            new_person.NickName=new TextFieldType();
                            new_person.NickName.Text = fbd_author.Nick;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.Email))
                        {
                            new_person.EMail=new TextFieldType();
                            new_person.EMail.Text = fbd_author.Email;
                        }
                        if(!string.IsNullOrEmpty(fbd_author.HomePages))
                        {
                            new_person.HomePage=new TextFieldType();
                            new_person.HomePage.Text = fbd_author.HomePages;
                        }
                         if(!string.IsNullOrEmpty(fbd_author.UID))
                        {
                            new_person.UID=new TextFieldType();
                            new_person.UID.Text = fbd_author.UID;
                        }
                         else
                         {
                            new_person.UID=new TextFieldType();
                            new_person.UID.Text =  id2author.ToString();
                         }
                        id2author++;
                        BookAuthors.Add(new_person);
                   }
                }
            }
        }

        public void CopyFromINF(PersonList Source)
         {
            foreach(LibPerson SP in Source.List)
            {
                List.Add(SP);
            }
         }

        public void ParseString(string Source)
        {
            string[] avtors = Source.Split(',');
            foreach (string s in avtors)
            {
                LibPerson pers = new LibPerson();
                string[] av = s.Split(new char[] { ' ', '.' }, 3);
                Array.Resize(ref av, 3);
                pers.First = string.IsNullOrEmpty(av[0]) ? string.Empty : av[0];
                pers.Last = string.IsNullOrEmpty(av[1]) ? string.Empty : av[1] ;
                pers.Middle = string.IsNullOrEmpty(av[2]) ? string.Empty : av[2];
                List.Add(pers);
            }
        }
    }



    public class LibPerson : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        
        public LibPerson(string last = "", string first = "", string middle = "", string nick = "", string homepages = "", string email = "", string avtortype = "")
        {
            _First = new LibLang(first);
            _Middle = new LibLang(middle);
            _Last = new LibLang(last);
            Nick = (nick == "" ? string.Empty : nick);
            HomePages = (homepages == "" ? string.Empty : homepages);
            Email = (email == "" ? string.Empty : email);
            PersonType = (avtortype == "" ? string.Empty : avtortype);
            UID = Guid.NewGuid().ToString();
            NotifyPropertyChanged("ListName");
        }
        public LibLang _First { get; private set; }
        public LibLang _Middle { get; private set; }
        public LibLang _Last { get; private set; }
        public string Nick { get; set; }
        public string HomePages { get; set; }
        public string Email { get; set; }
        public string PersonType { get; set; }
        public string UID { get; set; }
        public string First
        {
            get { return _First.BookLang; }
            set
            {
                _First.BookLang = value;
                NotifyPropertyChanged("ListName");
                NotifyPropertyChanged("First");
            }
        }
        public string Middle
        {
            get { return _Middle.BookLang; }
            set
            {
                _Middle.BookLang = value;
                NotifyPropertyChanged("ListName");
            }
        }
        public string Last
        {
            get { return _Last.BookLang; }
            set
            {
                _Last.BookLang = value;
                NotifyPropertyChanged("ListName");
                NotifyPropertyChanged("Last");
            }
        }
        public string FirstScr
        {
            get { return _First.ScrLang; }
            set { _First.ScrLang = value; }
        }
        public string MiddleScr
        {
            get { return _Middle.ScrLang; }
            set { _Middle.ScrLang = value; }
        }
        public string LastScr
        {
            get { return _Last.ScrLang; }
            set { _Last.ScrLang = value; }
        }

        public string ListName
        {
            get
            {
                return this.Last != string.Empty ? (this.Last + " " +
                          ((this.First.Trim().Length > 0) ? (this.First.Substring(0, 1) + ".") : "") +
                          ((this.Middle.Trim().Length > 0) ? (this.Middle.Substring(0, 1) + ".") : ""))
                                  : (this.Nick == string.Empty ? "Unknown" : this.Nick);
            }
        }
        //public int    indx      { get; set; }               
      
    }
}
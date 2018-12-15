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
    public class FBDInf : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public FBDInf(string fName="")
        {
            Book=new LibBook();
            FileInf = new LibFileInf(fName);
            Publisher = new LibPublisher();
            Fb2File = new FB2File();
        }
       
        public LibBook         Book        { get; private set; }
        public LibFileInf      FileInf     { get; private set; }
        public LibPublisher    Publisher   { get; private set; }

        protected FB2File Fb2File;

        public string FileName2Rename
        {
            get { 
                //return Book.FileName2Rename; 
                string retstr = string.Empty;
                string title_r = string.Empty;
                string year_r = string.Empty;
                foreach (LibPerson a in Book.Avtor.List)
                {
                    retstr += ", " + a.ListName;
                }
                if (!string.IsNullOrEmpty(retstr) && retstr.Length > 2)
                    retstr = retstr.Substring(2);
                else
                    retstr = string.Empty;    
                title_r = !string.IsNullOrEmpty(Book.Title) ? Book.Title : !string.IsNullOrEmpty(Publisher.Title) ? Publisher.Title : string.Empty;
                year_r = Publisher.Year != null ? " (" + Publisher.Year.ToString() + ")" : !string.IsNullOrEmpty(Book.s_BookDate) ? " (" + Book.s_BookDate + ")" : string.Empty;
                retstr = (!string.IsNullOrEmpty(retstr) ? retstr + " - " : string.Empty) + title_r + year_r;
                
                foreach(char c in Path.GetInvalidPathChars().Union(Path.GetInvalidFileNameChars()))
                    retstr = retstr.Replace(c, '.');
                return retstr;
            }
        }
        internal void FillFromFBD(Stream FileFBD)
        {
            //throw new NotImplementedException();
            FileFBD.Position = 0;
            try
            {
                XmlReader xread = XmlReader.Create(FileFBD);
                XDocument xd = XDocument.Load(xread);
                // FB2File Fb2 = new FB2File();
                Fb2File.Load(xd, false);
                Book.FillFromFBD(Fb2File);
                Publisher.FillFromFBD(Fb2File);
                FileInf.FillFromFBD(Fb2File);
            }
            catch { } ///TODO Обработка
           // Fb2File = Fb2;
        }

        internal void FillFromFileMask(StringMaskParser Mask)
        {
          //  FB2File Fb2 = new FB2File();
            //Book.FillFromFBD(Fb2File);
            //Publisher.FillFromFBD(Fb2File);
            //FileInf.FillFromFBD(Fb2File);
           // Fb2File = Fb2;
            Book.FillFromFileMask(Mask);
           
        }

        internal MemoryStream Return2FBD(bool OnlyHeader)
        {
            
            Book.Return2FBD(Fb2File);
            Publisher.Return2FBD(Fb2File);
            FileInf.Return2FBD(Fb2File);
            XDocument RetXMLFile = Fb2File.ToXML(OnlyHeader);
            MemoryStream toFile=new MemoryStream();
            RetXMLFile.Save(toFile);
            return toFile;
            // Fb2File = Fb2;
        }

        public void CopyFromINF(FBDInf Source)
        {
            Book.CopyFromINF(Source.Book);
            Publisher.CopyFromINF(Source.Publisher);

        }

    }

   

     public class LibLang
     {
         public LibLang(string name = "", string scrname = "")
         {
             BookLang = (name == "" ? string.Empty : name);
             ScrLang = (scrname == "" ? string.Empty : scrname);
         }
         public string BookLang    { get; set; }
         public string ScrLang { get; set; }
     }
    
    
    public class LibAnnot
    {
        public LibAnnot(string text="")
        {
            Text = (text == "" ? string.Empty : text);
        }
        public string Text { get; set; }
    }




   

}


/*////////////////////////////////////
 Что надо сделать FB2Library
--1. PersonType в авторы добавить
--2. Pages UDK BBK GRNTI добавить в издание
--3. добавить художника в издание? в книгу?
4. Для нескольких файлов контентную инфу

XXX. протащить через загрузку к себе

//////////////////////////////////*/


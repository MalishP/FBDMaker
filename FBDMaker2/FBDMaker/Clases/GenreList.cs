using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.ComponentModel;

namespace FBDMaker
{
    public class GenreList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public GenreList()
        {
            
            Glist=new ObservableCollection<GenreNode>();
            this.FillListFromXML();
        }
        //public GenreList(string pathXML)
        //{
        //    Glist=new ObservableCollection<GenreNode>();
        //    this.FillFromXML(pathXML);
        //}
       
        public ObservableCollection<GenreNode> Glist {get;private set;}
        public void FillListFromXML()//string path)
        {
            Glist.Clear();
            XDocument XDocGen = XDocument.Load("dat\\GenreList.xml", LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            XElement XElGen = XDocGen.Element("GenreList");
            foreach (XElement xel in XElGen.Nodes().OfType<XElement>())
            {
                GenreNode GNode = new GenreNode(xel.Attribute("Name").Value, (xel.Name == "Title" ? "" : xel.Attribute("Kod").Value));
                GNode.FillFromXML(xel);
                Glist.Add(GNode);
            }
        }

        public List <string> GetCheckedGenreList( )
        {
            List<string> ListGenre = new List<string>();
            foreach (GenreNode gn in Glist.Where(s=>s.IsChecked||s.IsChekedChaild))
            {
                if (gn.IsChecked&gn.IsGenre)
                    ListGenre.Add(gn.Code);
                if(gn.IsChekedChaild)
                    gn.FillCheckedGenreList(ListGenre);
            }
            return ListGenre;

        }
        public void ResetExpand()
        {
            foreach(GenreNode gn in Glist)
            {
                gn.ResetExpand();
            }

        }
        public string SelectedNameList
        {
            get {
                string s="";
                foreach (GenreNode gn in Glist)
                {
                    s += gn.SelectedNameList ;
                }
                return s;
            }
        }

        public void CheckChange(Boolean direct)
        {
            foreach (GenreNode gn in Glist)
            {
                gn.CheckChange(direct);
            }
            NotifyPropertyChanged("SelectedNameList");
            
        }
        public void ChangeAccept()
        {
            CheckChange(true);
        }
        public void ChangeCancel()
        {
            CheckChange(false);
        }
        public void CheckGenre(String checkedGenre)
        {
            foreach (GenreNode gn in Glist)
            {
                gn.CheckGenre(checkedGenre);
            }
            NotifyPropertyChanged("SelectedNameList");
        }
        
    }

    public class GenreNode
    {
        public GenreNode(string name, string code = "")
        {
            Name = name;
            Code = (code == "" ? string.Empty : code);
            IsChecked = false;
            IsCheckVis = IsChecked;
            IsGenre = (code == "" ? false : true);
            Chaild = new ObservableCollection<GenreNode>();
            Expand = false;
        }
        public string Name {get;set;}
        public string Code{get;set;}
        public Boolean IsChecked { get; set; }
        public Boolean IsCheckVis { get; set; }
        public Boolean IsGenre { get; set; }
        public ObservableCollection<GenreNode> Chaild { get; private set; }
        public Boolean Expand { get; set; }

        public Boolean IsChekedChaild
        {
            get
            {
                return (Chaild.Any(ch=>(ch.IsChecked ||ch.IsChekedChaild)));
            }
        }

        public void FillCheckedGenreList(List <string> ListGenre)
        {
            foreach (GenreNode gn in Chaild.Where(s=>s.IsChecked||s.IsChekedChaild))
            {
                if (gn.IsChecked&&gn.IsGenre)
                    ListGenre.Add(gn.Code);
                if (gn.IsChekedChaild)
                    gn.FillCheckedGenreList(ListGenre);
            }
            

        }
        public string SelectedNameList
        {
            get
            {
                string s = IsChecked ? Name + "; " : "";
                foreach (GenreNode gn in Chaild )
                {
                    s += gn.SelectedNameList;
                }
                return s;
            }
        }
        public void FillFromXML(XElement XElNode)
        {
            foreach (XElement xel in XElNode.Nodes().OfType<XElement>())
            {
                GenreNode GNode = new GenreNode(xel.Attribute("Name").Value, (xel.Name == "Title" ? "" : xel.Attribute("Kod").Value));
                GNode.FillFromXML(xel);
                Chaild.Add(GNode);
            }
        }
        public void CheckChange(Boolean direct)
        {
            if(direct)
            {
                IsChecked=IsCheckVis;
            }
            else
            {
                IsCheckVis=IsChecked;
            }
            foreach (GenreNode gn in Chaild )
            {
                gn.CheckChange(direct);
            }
        }
         public void ResetExpand()
        {
            Expand = IsChekedChaild;
            foreach(GenreNode gn in Chaild)
            {
                gn.ResetExpand();
            }

        }
         public void CheckGenre(String checkedGenre)
         {
             IsChecked = Code.Equals(checkedGenre)||IsChecked;
             foreach (GenreNode gn in Chaild)
             {
                 gn.CheckGenre(checkedGenre);
             }
         }
    }
}

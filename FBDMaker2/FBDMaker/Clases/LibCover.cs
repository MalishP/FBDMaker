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
    public class LibCover : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public LibCover()
        {
            //Base64 = string.Empty;
            BinaryData = null;
            CoverName = string.Empty;
            CoverType=string.Empty;
        }
        //public string Base64 { get; set; }
        //public ImageSource CoverIS { get; set; }
        public Byte[] BinaryData { get; set; }
        public string CoverName {get;set;}
        public string CoverType { get; set; }
        public ImageSource Cover
        {
            get
            {
                BitmapImage bmpImage;
                if (BinaryData != null)
                {
                    bmpImage = new BitmapImage();
                    MemoryStream strm = new MemoryStream(BinaryData);
                    bmpImage.BeginInit();
                    strm.Seek(0, SeekOrigin.Begin);
                    bmpImage.StreamSource = strm;
                    bmpImage.EndInit();
                }
                else
                {
                    bmpImage = new BitmapImage(new Uri("dat\\unnamed.jpg", UriKind.Relative));
                }
                return bmpImage;
            }
        }

        public void FillFromFile(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            CoverName = Path.GetFileName(fileName);
            CoverType = Path.GetExtension(fileName);
            BinaryData = new Byte[fs.Length];
            fs.Read(BinaryData, 0, BinaryData.Length);
            fs.Close();
            fs.Dispose();
            NotifyPropertyChanged("Cover");
        }

        public void Clear()
        {
            BinaryData = null;
            CoverName = string.Empty;
            NotifyPropertyChanged("Cover");

        }

        internal void FillFromFBD(FB2File FBD)
        {
            if (FBD.TitleInfo.Cover != null)
            {
                foreach (InlineImageItem coverImag in FBD.TitleInfo.Cover.CoverpageImages)
                {
                    if (string.IsNullOrEmpty(coverImag.HRef))
                    {
                        continue;
                    }
                    string coverref;

                    if (coverImag.HRef.Substring(0, 1) == "#")
                    {
                        coverref = coverImag.HRef.Substring(1);
                    }
                    else
                    {
                        coverref = coverImag.HRef;
                    }
                    CoverName = coverref;
                    BinaryData = null;
                    BinaryData = new byte[FBD.Images[coverref].BinaryData.Length];
                    FBD.Images[coverref].BinaryData.CopyTo(BinaryData, 0); ////править 
                }
                NotifyPropertyChanged("Cover");
            }
        }
        internal void Return2FBD(FB2File FBD)
        {
            
            if (!string.IsNullOrEmpty(CoverName))
            {
                if(FBD.TitleInfo.Cover == null)
                    FBD.TitleInfo.Cover = new CoverPage();
                InlineImageItem image = new InlineImageItem();
                image.AltText=CoverName;
                image.HRef = CoverName;
                image.ImageType = CoverType;
                FBD.TitleInfo.Cover.CoverpageImages.Clear();
                FBD.TitleInfo.Cover.CoverpageImages.Add(image);

                BinaryItem item = new BinaryItem();
                switch (CoverType.ToLower())
                {
                    case ".jpeg":
                    case ".jpg":
                        item.ContentType= ContentTypeEnum.ContentTypeJpeg;
                        break;
                    case ".png":
                        item.ContentType = ContentTypeEnum.ContentTypePng;
                        break;
                    case ".gif":
                        item.ContentType = ContentTypeEnum.ContentTypeGif;
                        break;
                    default:
                        item.ContentType = ContentTypeEnum.ContentTypeJpeg;
                        break;
                }
                item.Id = CoverName;
                item.BinaryData = new byte[BinaryData.Length];
                BinaryData.CopyTo(item.BinaryData, 0);

                FBD.AddImages(item);
            }
        }
    }
}

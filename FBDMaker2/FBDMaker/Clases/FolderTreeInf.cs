using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows ;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using SevenZip;
using System.Diagnostics;
using DjvuNet;
using DjvuNet.Graphics;
using System.Drawing.Imaging;
using PdfiumLight;//PDFLibNet;
using System.Collections.ObjectModel;


namespace FBDMaker
{
    public class FolderTreeInf
    {
        
        public FolderTreeInf(string fname , string fpath ,ImageSource img=null, Boolean ischeck=false)
        {
            FName = fname;
            FPath = fpath;
            FType = FBDMaker.Properties.Resources.Dir;
            IsChecked = ischeck;
            Img = img;
            IsFolder = true;
            Node = new ObservableCollection<FolderTreeInf>();
            LibInfo = new FBDInf(fname);
            IsFill = false;
        }

        public FolderTreeInf()
        {
            FName = string.Empty;
            FPath = string.Empty;
            FType = string.Empty;
            IsChecked = false; ;
            IsFolder = true;
            Node = new ObservableCollection<FolderTreeInf>();
            Img = null;
            LibInfo = new FBDInf();
            IsFill = false; 
        }

        public FolderTreeInf(FileSystemInfo fi, Boolean ischeck = false)
        {
            FName = fi.Name;
            FPath = fi.FullName;
            FType = (fi.Attributes&FileAttributes.Directory)==FileAttributes.Directory ?  FBDMaker.Properties.Resources.Dir : fi.Extension;
            IsChecked = ischeck;
            IsFolder = (fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory ? true : false;
            LibInfo = new FBDInf(FName);
            Img = ShellIcon.ImageSourceLargeIcon(FPath); //ShellIcon.ImageSource4Icon(Icon.ExtractAssociatedIcon(fi.FullName));
            Node = new ObservableCollection<FolderTreeInf>();
            IsFill = false;
        }
        private string ParsedFile=string.Empty;
        private bool ParsedFileInArch = false;
        private int N_image = 1;
        private string CoverPath=string.Empty;
        private bool CreateFDB = true;
        public string FName { get; set; }
        public string FPath {get;set;}
        public string FType { get; set; }
        public Boolean IsFill { get; set; }
        public Boolean IsChecked {get;set;}
        public ObservableCollection<FolderTreeInf> Node { get; private set; }
        public ImageSource Img { get; set; }
        public Boolean IsFolder { get;private set; }
 
        public FBDInf LibInfo { get; private set; }

        public Boolean IsArch
        {
            get
            {
                return (FType.ToLower() == ".zip" || FType.ToLower() == ".7z" || FType.ToLower() == ".7zip" || FType.ToLower() == ".rar");                
            }
        }
        public Boolean HasChaildFolder
        {
            get
            {
                if (!IsFolder)
                    return false;
                try
                {
                    string[] root = Directory.GetDirectories(FPath);
                    if (root.Length > 0)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }
        }
        public Boolean HasChaildFile
        {
            get
            {
                if (!IsFolder)
                    return false;
                try
                {
                    string[] root = Directory.GetFiles(FPath);
                    if (root.Length > 0)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }
        }
        public Boolean HasChaild
        {
            get
            {
                if (!IsFolder)
                    return false;
                if (HasChaildFile || HasChaildFolder)
                    return true;
                else
                    return false;
            }
        }

        public static void GetTopItem(ICollection<FolderTreeInf> ListNode) //<ExplorerNode>
        {
            //List<TreeListNode> ListNode = new List<TreeListNode>();
            FolderTreeInf FDT = new FolderTreeInf(FBDMaker.Properties.Resources.Desktop, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            //TreeListNode DT = new TreeListNode() { Content = FDT };
            //FDT.Node=DT;
            FDT.Img=ShellIcon.ImageSourceLargeIcon(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            if (FDT.HasChaildFolder)
            {
                FDT.Node.Add(new FolderTreeInf());
            }
            ListNode.Add(FDT);

            FolderTreeInf FMD=new FolderTreeInf(FBDMaker.Properties.Resources.MyDoc, Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            FMD.Img = ShellIcon.ImageSourceLargeIcon(FMD.FPath);
            if (FMD.HasChaildFolder)
            {
                FMD.Node.Add(new FolderTreeInf());
            }
            ListNode.Add(FMD);
            String s = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            if (Directory.Exists(s))
            {
                
                FolderTreeInf FDD=new FolderTreeInf(FBDMaker.Properties.Resources.Downloads,s);
                FDD.Img = ShellIcon.ImageSourceLargeIcon(FDD.FPath);
                if (FDD.HasChaildFolder)
                {
                    FDD.Node.Add(new FolderTreeInf());
                }
                ListNode.Add(FDD);
            }
            String[] ld = Directory.GetLogicalDrives();
            foreach (string sd in ld)
            {
                FolderTreeInf FLD=new FolderTreeInf(sd,sd);
                FLD.Img = ShellIcon.ImageSourceLargeIcon(FLD.FPath);
                if (FLD.HasChaildFolder)
                {
                    FLD.Node.Add(new FolderTreeInf());
                }
                ListNode.Add(FLD);
            }
           // return ListNode;
        }


        public static FolderTreeInf GetFillNode(FileSystemInfo di, Boolean FoldersOnly = true)
        {
            FolderTreeInf FLD = new FolderTreeInf(di);
            //TreeListNode LD = new TreeListNode(FLD);
            //FLD.Node = LD;
            if (FoldersOnly)
            {
                if (FLD.HasChaildFolder)
                    FLD.Node.Add(new FolderTreeInf());
            }
            else
                if (FLD.HasChaild)
                {
                    FLD.Node.Add(new FolderTreeInf());
                }
             
            return FLD;

        }

        public static void ReReadListFields(ICollection<FolderTreeInf> ListNode, String folderpath)
        {
            List<FolderTreeInf> Copy_ListNode = ListNode.ToList();
            foreach (FolderTreeInf fti in Copy_ListNode)
            {
                if (fti.IsFolder && !Directory.GetDirectories(folderpath).Where(s=>s==fti.FPath).Any())
                {
                    ListNode.Remove(fti);
                }
               if(!fti.IsFolder && !Directory.GetFiles(folderpath).Where(s=>s==fti.FPath).Any())
               {
                   ListNode.Remove(fti);
               }
            }
            string[] root = Directory.GetDirectories(folderpath);
            foreach (string s in root)
            {
                if(!ListNode.Where(ln=>ln.FPath==s&&ln.IsFolder).Any())
                {
                    DirectoryInfo di = new DirectoryInfo(s);
                    FolderTreeInf LD = GetFillNode(di, false);

                    ListNode.Add(LD);
                }
            }
            root = Directory.GetFiles(folderpath);
            foreach (string s in root)
            {
                if (!ListNode.Where(ln => ln.FPath == s && !ln.IsFolder).Any())
                {

                    FileInfo di = new FileInfo(s);
                    FolderTreeInf LD = GetFillNode(di);

                    ListNode.Add(LD);
                   
                }
            }
        }
        public static void GetListItem(ICollection<FolderTreeInf> ListNode, String folderpath)
        {
            GetListFolders(ListNode,folderpath,false);
            GetListFiels(ListNode,folderpath);
           
        }


        public static void GetListFolders(ICollection<FolderTreeInf> ListNode, String folderpath, Boolean FoldersOnly = true)
        {
          
            DirectoryInfo di;
            ListNode.Clear();
            try
            {
                string[] root = Directory.GetDirectories(folderpath);
                foreach (string s in root)
                {
                    try
                    {
                        di = new DirectoryInfo(s);
                        FolderTreeInf LD = GetFillNode(di, FoldersOnly);
                        
                        ListNode.Add(LD);
                    }
                    catch { }
                }
            }
            catch { }
           // return ListNode;
        }

       

        public static void GetListFiels(ICollection<FolderTreeInf> ListNode, String folderpath)
        {
            //List<TreeListNode> ListNode = new List<TreeListNode>();
            
            FileInfo di;
            try
            {
                string[] root = Directory.GetFiles(folderpath);
                foreach (string s in root)
                {
                    try
                    {
                        di = new FileInfo(s);
                        FolderTreeInf LD = GetFillNode(di);
                        
                        ListNode.Add(LD);
                    }
                    catch { }
                }
            }
            catch { }
            ///return ListNode;
        }

        public void OpenFolders(Boolean FoldersOnly=true)
        {

            GetListFolders(Node, FPath, FoldersOnly);
            
        }

        public void OpenFiles()
        {
            GetListFiels(Node, FPath);
            
        }

        public void OpenNode(Boolean FoldersOnly = true)
        {
            Node.Clear();
            OpenFolders(FoldersOnly);
            if (!FoldersOnly)
                OpenFiles();
        }

        private void AddFileToArch(string ArchName, string AddFileName, bool PreserveDirectoryRoot=true )
        {
            string ArchType = Path.GetExtension(ArchName);
            if (ArchType.ToLower() == ".zip" || ArchType.ToLower() == ".7z" || ArchType.ToLower() == ".7zip")
            {
                SevenZipCompressor FileCompres = new SevenZipCompressor();
                FileCompres.CompressionMode = File.Exists(ArchName) ? CompressionMode.Append : CompressionMode.Create;
                

                switch (ArchType.ToLower())
                {
                    case ".zip":
                        FileCompres.ArchiveFormat = OutArchiveFormat.Zip;
                        break;
                    default:
                        FileCompres.ArchiveFormat = OutArchiveFormat.SevenZip;
                        break;
                }
                
                string[] listADD = new string[] { AddFileName };
                if ((File.GetAttributes(AddFileName)&FileAttributes.Directory) == FileAttributes.Directory)
                {
                    FileCompres.PreserveDirectoryRoot = PreserveDirectoryRoot;
                    FileCompres.CompressDirectory(AddFileName, ArchName);
                }
                else
                {
                    FileCompres.CompressFiles(ArchName, listADD);
                }
            }
            if (FType.ToLower() == ".rar")
            {
                Process RarProc = new Process();
                RarProc.StartInfo.FileName = Environment.CurrentDirectory + "\\rar.exe";
                RarProc.StartInfo.Arguments = "a -ep " + ArchName + " " + AddFileName; //-r 
                RarProc.StartInfo.UseShellExecute = false;
                RarProc.Start();
                RarProc.WaitForExit();
            }

        }

        private void SaveFBDFile(string FBDFileName, bool OnlyHeader)
        {
            FileStream filewrite_fb2 = File.Create(FBDFileName);
            MemoryStream memFBD_fb2;
            memFBD_fb2 = LibInfo.Return2FBD(OnlyHeader);
            memFBD_fb2.Position = 0;
            memFBD_fb2.CopyTo(filewrite_fb2);
            memFBD_fb2.Close();
            memFBD_fb2.Dispose();
            filewrite_fb2.Close();
            filewrite_fb2.Dispose();
        }
        //TODO добавит нормальную обработку "старых" fbd в архивах в папке
        public void SaveLibInf(WorkOptions Option)
        {
           // MemoryStream memFBD ;
            //bool OnlyHeader=true;
            bool DeleteFBDFile = false;
            string pathFBD;
            string ParsedFullDir = string.Empty;
            string ParsedRootDir = string.Empty;

            if (!string.IsNullOrEmpty(ParsedFile) && ParsedFileInArch )
            {
                if(!string.IsNullOrEmpty(Path.GetDirectoryName(ParsedFile)))
                {
                    ParsedFullDir = Path.GetDirectoryName(ParsedFile);
                    ParsedRootDir=Path.GetDirectoryName(ParsedFile);

                    while(!string.IsNullOrEmpty(Path.GetDirectoryName(ParsedRootDir)))
                    {
                        ParsedRootDir = Path.GetDirectoryName(ParsedRootDir);
                    }
                    ParsedRootDir=Path.Combine(Path.GetTempPath(),ParsedRootDir);
                    Directory.CreateDirectory(Path.Combine(Path.GetTempPath() , ParsedFullDir));
                    
                }
            }

            if (Path.GetExtension(ParsedFile) == ".fb2" )
            {
                string Fb2File = ParsedFileInArch ? Path.GetTempPath() + ParsedFile : ParsedFile;
                SaveFBDFile(Fb2File, false);
                ParsedFile = string.Empty;
                if (ParsedFileInArch)
                {
                    AddFileToArch(FPath, (ParsedRootDir == string.Empty ? Fb2File : ParsedRootDir));
                    File.Delete(Fb2File);
                }

            }

            if(Option.RenameFiles_Yes)
            {
                string NewName = string.Empty;
                NewName = LibInfo.FileName2Rename;

                //NewName += !string.IsNullOrEmpty(LibInfo.Publisher.Title) ? LibInfo.Publisher.Title : !string.IsNullOrEmpty(LibInfo.Book.Title) ? LibInfo.Book.Title : string.Empty;
                //NewName += LibInfo.Publisher.Year != null ? " (" + LibInfo.Publisher.Year.ToString() + ")" : string.Empty;
                if (!string.IsNullOrEmpty(NewName))
                {
                    string NewFullName=Path.Combine(Path.GetDirectoryName(FPath),NewName+(IsFolder?string.Empty: FType ));
                    string oldFpath=FPath;
                    if (NewFullName != FPath)
                    {
                        try
                        {
                            Directory.Move(FPath, NewFullName);
                            FPath = NewFullName;
                            FName = NewName + (IsFolder ? string.Empty : FType);

                            if (!string.IsNullOrEmpty(ParsedFile) && !ParsedFileInArch)
                            {
                                if (ParsedFile.Contains(oldFpath))
                                    ParsedFile = ParsedFile.Replace(oldFpath, FPath);
                                string NewParsedFile = Path.Combine(Path.GetDirectoryName(ParsedFile), NewName + Path.GetExtension(ParsedFile));
                                File.Move(ParsedFile, NewParsedFile);
                                ParsedFile = NewParsedFile;
                            }
                        }
                        catch { }//TODO добавить обработку
                    }
                }
                
            }

            if (CreateFDB)
            {
                if (!string.IsNullOrEmpty(ParsedFile))
                {
                    if (ParsedFileInArch)
                    {

                        pathFBD = Path.GetTempPath() + ParsedFile;
                        DeleteFBDFile = true;
                    }
                    else
                    {
                        pathFBD = ParsedFile;
                    }
                    
                }
                else
                {
                    if (Option.MakeArch_Yes && !IsArch)
                    {
                        pathFBD = LibInfo.FileName2Rename; //string.Empty;
                        //pathFBD = LibInfo.Book.Avtor.List.Count != 0 ? LibInfo.Book.Avtor.List.First().ListName + " - " : pathFBD;
                        //pathFBD += !string.IsNullOrEmpty(LibInfo.Book.Title) ? LibInfo.Book.Title : string.Empty;
                        pathFBD += !string.IsNullOrEmpty(pathFBD) ? ".fbd" : string.Empty;
                        pathFBD=!string.IsNullOrEmpty(pathFBD) ? Path.GetDirectoryName(FPath) + "\\"+pathFBD:Path.ChangeExtension(FPath, ".fbd");
                    }
                    else
                    {
                        pathFBD = Path.ChangeExtension(FPath, ".fbd");
                    }
                }
                SaveFBDFile(pathFBD, true);
               
                if (Option.MakeArch_Yes && !IsArch)
                {
                    //SevenZipCompressor FileCompres = new SevenZipCompressor();
                    //FileCompres.ArchiveFormat = OutArchiveFormat.Zip;
                    //FileCompres.CompressionMode = CompressionMode.Create;
                    //string[] listADD = new string[] { FPath };
                    string NameArch = LibInfo.FileName2Rename;//string.Empty;
                    //NameArch = LibInfo.Book.Avtor.List.Count != 0 ? LibInfo.Book.Avtor.List.First().ListName + " - " : NameArch;
                    //NameArch += !string.IsNullOrEmpty(LibInfo.Book.Title) ? LibInfo.Book.Title : string.Empty;
                    NameArch += !string.IsNullOrEmpty(NameArch) ? ".zip" : Path.ChangeExtension(Path.GetFileName(FPath), ".zip");
                    string ArchPath = Path.GetDirectoryName(FPath) + "\\" + NameArch;
                    AddFileToArch(ArchPath, FPath, Option.FolderRootAddArch_Yes);
                    if(Option.InsertFBDArch_Yes)
                    {
                        AddFileToArch(ArchPath, pathFBD, Option.FolderRootAddArch_Yes);
                        DeleteFBDFile = Option.DeleteAfterArch_Yes;
                    }
                    else
                    {
                        File.Move(pathFBD, Path.ChangeExtension(ArchPath, ".fbd"));
                        pathFBD = Path.ChangeExtension(ArchPath, ".fbd");
                    }
                    if(Option.DeleteAfterArch_Yes)
                    {
                        try
                        {
                            if (IsFolder) { Directory.Delete(FPath, true); }
                            else { File.Delete(FPath); }
                        }
                        catch { };//TODO добавить обработку
                    }
                    //FileCompres.CompressFiles(ArchPath, listADD);
                    
                }

                if ((ParsedFileInArch || Option.InsertFBDArch_Yes)&&IsArch)
                {
                    AddFileToArch(FPath, (ParsedRootDir == string.Empty ? pathFBD : ParsedRootDir));
                }

                if (DeleteFBDFile && File.Exists(pathFBD))
                    File.Delete(pathFBD);
            }
        }
        public void FillLibInf(StringMaskParser  Mask )
        {
            if (!IsFill)
            {
                MemoryStream FileFBD=new MemoryStream();

                if (FBFile2tMemStream(FileFBD))
                {
                    LibInfo.FillFromFBD(FileFBD);//
                    FileFBD.Close();
                    FileFBD.Dispose();
                }
                else
                {
                    Mask.Parse((IsFolder ? FName : Path.GetFileNameWithoutExtension(FName)));
                    LibInfo.FillFromFileMask(Mask);
                    FillCoverNextPage();
                };

                IsFill = true;
            }
        }

        //CoverPath
        public void FillCoverNextPage(string CoverPathImg="")
        {
            bool isNewFileCover = !string.IsNullOrEmpty(CoverPathImg);
            CoverPath = string.IsNullOrEmpty(CoverPath)? FPath : CoverPath;
            CoverPathImg = string.IsNullOrEmpty(CoverPathImg) ? CoverPath : CoverPathImg;

            string CoverFType = Path.GetExtension(CoverPathImg);
          

            if (CoverFType.ToLower() == ".djvu" || CoverFType.ToLower() == ".djv" || CoverFType.ToLower() == ".pdf" || CoverFType.ToLower() == ".jpeg" || CoverFType.ToLower() == ".jpg" || CoverFType.ToLower() == ".png" || CoverFType.ToLower() == ".gif")
            {
                N_image = isNewFileCover ? 1 : N_image;
                string tmppath = Path.GetTempPath() + "tmpImg.jpg";
                string tmpImpath = Path.GetTempPath() + "Cover.jpg";
             
                if (CoverFType.ToLower() == ".jpeg" || CoverFType.ToLower() == ".jpg" || CoverFType.ToLower() == ".png" || CoverFType.ToLower() == ".gif")
                {
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(CoverPathImg);
                    bitmap.Save(tmppath, ImageFormat.Jpeg);
                    
                    CoverPath = string.Empty;
                }

                if (CoverFType.ToLower() == ".pdf")
                {

                    //PDFWrapper doc = new PDFWrapper();
                    //doc.LoadPDF(CoverPathImg);
                    //doc.ExportJpg(tmppath, N_image, N_image, 150, 90, -1);
                    PdfDocument document = new PdfDocument(CoverPathImg);
                    PdfPage page = document.GetPage(N_image-1);
                    var renderedPage = page.Render(90,150);
                    renderedPage.Save(tmppath, ImageFormat.Jpeg);
                    CoverPath = CoverPathImg;
                    if (N_image == 3)
                        N_image = 1;
                    else N_image++;
                    page.Dispose();
                    document.Dispose();
                }
                if (CoverFType.ToLower() == ".djvu" || CoverFType.ToLower() == ".djv")
                {
                    DjvuDocument doc = new DjvuDocument(CoverPathImg);

                    var page = doc.Pages[N_image-1];

                    page
                        .BuildPageImage()
                        .Save(tmppath, ImageFormat.Jpeg);
                    CoverPath = CoverPathImg;
                    if (N_image == 3)
                        N_image = 1;
                    else N_image++;
                   
                }
                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(tmppath))
                {
                    System.Drawing.Size size = new System.Drawing.Size(bitmap.Width * 640 / bitmap.Height, 640);
                    using (System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(bitmap, size))
                    {
                        //  File.Delete(tmppath);
                        newBitmap.Save(tmpImpath, ImageFormat.Jpeg);
                    }
                }
                LibInfo.Book.Cover.FillFromFile(tmpImpath);
                 File.Delete(tmppath);
                
                File.Delete(tmpImpath);
               
                
            }
        }

        private Boolean FBFile2tMemStream (MemoryStream FileFBD)
        {
            Boolean isNotStartFill = true;
            string PathFBD=string.Empty;
            if (!IsFolder)
            {
                    
                if (IsArch)
                {
                        
                    //FileStream tempstr = File.OpenRead(FPath);
                    //MemoryStream tmpMemStrea = new MemoryStream();
                    //tempstr.CopyTo(tmpMemStrea);           //FileStream(PathFBD, FileMode.Open); 

                    SevenZipExtractor se = new SevenZipExtractor(FPath); //new SevenZipExtractor(tmpMemStrea);
                    //tempstr.Close();
                    //tempstr.Dispose();
                    if (se.ArchiveFileNames.Where(sf => Path.GetExtension(sf) == ".fbd").Any())
                    {
                        foreach (string s in se.ArchiveFileNames.Where(sf => Path.GetExtension(sf) == ".fbd")) //.First()
                        {
                            se.ExtractFile(s, FileFBD);
                            isNotStartFill = false;
                            ParsedFile = s;
                            ParsedFileInArch = true;
                            break;
                        }
                    }
                    else 
                    {
                        if (se.ArchiveFileNames.Where(sf => Path.GetExtension(sf) == ".fb2").Any())
                        {
                            foreach (string s in se.ArchiveFileNames.Where(sf => Path.GetExtension(sf) == ".fb2")) //.First()
                            {
                                se.ExtractFile(s, FileFBD);
                                isNotStartFill = false;
                                ParsedFile = s;
                                ParsedFileInArch = true;
                                CreateFDB = se.ArchiveFileNames.Count ==1 ? false : true;
                                break;
                            }
                        }
                    }
                    se.Dispose();
                    //tmpMemStrea.Close();
                    //tmpMemStrea.Dispose();
                }//вытаскивание в поток из архива
                else
                    if (FType.ToLower() == ".fb2")
                    {
                        PathFBD = FPath;
                        CreateFDB = false;
                    }
                    else
                    {
                        PathFBD = File.Exists(Path.ChangeExtension(FPath, ".fbd")) ? Path.ChangeExtension(FPath, ".fbd") : string.Empty;
                    };
            }
            else
            {
                PathFBD = File.Exists(Path.Combine(FPath,FName + ".fbd")) ? Path.Combine(FPath,FName + ".fbd") : string.Empty;
                if (PathFBD == string.Empty)
                {
                    PathFBD = File.Exists(Path.ChangeExtension(FPath, ".fbd")) ? Path.ChangeExtension(FPath, ".fbd") : string.Empty;
                }
                if (PathFBD == string.Empty)
                {
                    if (Directory.GetFiles(FPath, "*.fbd").Any())
                    {
                        PathFBD = Directory.GetFiles(FPath, "*.fbd").First();
                    }
                    else
                    {
                        if (Directory.GetFiles(FPath, "*.fb2").Any())
                        {
                            PathFBD = Directory.GetFiles(FPath, "*.fb2").First();
                            CreateFDB = Directory.GetFiles(FPath).Count() == 1 ? false : true;
                        }
                    }
                }
                if (PathFBD == string.Empty)
                {
                    PathFBD = File.Exists(Path.Combine(Path.GetDirectoryName(FPath), FName + ".fbd")) ? Path.Combine(Path.GetDirectoryName(FPath), FName + ".fbd") : string.Empty;
                };
            };
            if (PathFBD != string.Empty)
            {
                // FileFBD = new FileStream();
                FileStream tempstr = File.OpenRead(PathFBD);
                tempstr.CopyTo(FileFBD);           //FileStream(PathFBD, FileMode.Open); 
                tempstr.Close();
                tempstr.Dispose();
                ParsedFile = PathFBD;
                isNotStartFill = false;
            }; // в поток из файлв
            return !isNotStartFill;
        }
        public void MultiSave(WorkOptions Option)
        {
            if (Option.MultySelOperation_Copy)
            { 
                //копирование в файлы
                foreach(FolderTreeInf FTIDest in Node)
                {
                    FTIDest.LibInfo.CopyFromINF(LibInfo);
                }
            }
            else { 
            //архивирование файлов
                bool DeleteFBDFile=false;
                string fmpath = Node.First().FPath;
                string pathFBD = string.Empty;
                
                pathFBD = LibInfo.Book.Avtor.List.Count != 0 ? LibInfo.Book.Avtor.List.First().ListName + " - " : pathFBD;
                pathFBD += !string.IsNullOrEmpty(LibInfo.Book.Title) ? LibInfo.Book.Title : string.Empty;
                pathFBD += !string.IsNullOrEmpty(pathFBD) ? ".fbd" : pathFBD = Path.ChangeExtension(fmpath, ".fbd");
                      
                SaveFBDFile(pathFBD, true);
               
                string NameArch = string.Empty;
                NameArch = LibInfo.Book.Avtor.List.Count != 0 ? LibInfo.Book.Avtor.List.First().ListName + " - " : NameArch;
                NameArch += !string.IsNullOrEmpty(LibInfo.Book.Title) ? LibInfo.Book.Title : string.Empty;
                NameArch += !string.IsNullOrEmpty(NameArch) ? ".zip" : Path.ChangeExtension(Path.GetFileName(fmpath), ".zip");
                string ArchPath = Path.GetDirectoryName(fmpath) + "\\" + NameArch;
                foreach(FolderTreeInf fi in Node)
                {
                        AddFileToArch(ArchPath, fi.FPath,true);
                }
                if(Option.InsertFBDArch_Yes)
                {
                    AddFileToArch(ArchPath, pathFBD, Option.FolderRootAddArch_Yes);
                    DeleteFBDFile = Option.DeleteAfterArch_Yes;
                }
                else
                {
                    File.Move(pathFBD, Path.ChangeExtension(ArchPath, ".fbd"));
                    pathFBD = Path.ChangeExtension(ArchPath, ".fbd");
                }
                if(Option.DeleteAfterArch_Yes)
                {
                    foreach(FolderTreeInf fi in Node)
                    {
                        if (fi.IsFolder) { Directory.Delete(fi.FPath);}
                         else { File.Delete(fi.FPath); }
                     }
                    
                }
                        
                if (DeleteFBDFile)
                      File.Delete(pathFBD);
            }
        }
    }
}

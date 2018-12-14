using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DjvuNet;
using PdfiumLight;//PDFLibNet;


namespace FBDMaker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        public ObservableCollection<FolderTreeInf> nodes;
        public ObservableCollection<FolderTreeInf> nodes2;
        public ObservableCollection<FolderTreeInf> nodes3;
        public WorkOptions Option;
        public FolderTreeInf MultiSel;

        public MainWindow()
        {
            InitializeComponent();
            nodes = new ObservableCollection<FolderTreeInf>();
            nodes2 = new ObservableCollection<FolderTreeInf>();
            Option = new WorkOptions();
            //nodes3 = new List<FolderTreeInf>();
            FolderTreeInf.GetTopItem(nodes);
            //FolderTreeInf.GetTopItem(nodes2);
            //FolderTreeInf.GetTopItem(nodes3);
            treeView1.ItemsSource = nodes;
            treeView2.ItemsSource = nodes2;
            expander_param.DataContext = Option;
           //TextBox1.DataContext = Option.Mask.Mask;
            //treeViewt.ItemsSource = nodes3;

        }

        private void treeView1_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            (item.Header as FolderTreeInf).OpenNode();
           // (treeView1.SelectedItem as FolderTreeInf).OpenNode();
        }

        private void treeView2_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            (item.Header as FolderTreeInf).OpenNode(false);
            // (treeView1.SelectedItem as FolderTreeInf).OpenNode();
        }

        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FolderTreeInf item = (FolderTreeInf)e.NewValue;
            treeView2.ItemsSource = null;
            nodes2.Clear();
            FolderTreeInf.GetListItem(nodes2, item.FPath);
            treeView2.ItemsSource = nodes2;
        }

        private void treeView2_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FolderTreeInf item = (FolderTreeInf)e.NewValue;
          //  FolderTreeInf itemold = (FolderTreeInf)e.OldValue;
            if (item != null)
            {
                item.FillLibInf(this.Option.Mask);
            }
            //if (itemold != null)
            //{
            //    itemold.SaveLibInf();
            //       //FillLibInf(this.Option.Mask);
            //}
        }

        private void AddAvtor_Click(object sender, RoutedEventArgs e)
        {
           // ( (FolderTreeInf)treeView2.SelectedItem).LibInfo.Avtor.Add(new LibPerson());
            System.Windows.Controls.Button ClickButton = (System.Windows.Controls.Button)sender;
            ObservableCollection<LibPerson> listpers = (ObservableCollection<LibPerson>)ClickButton.DataContext;
            LibPerson pers = new LibPerson();
            listpers.Add(pers);
        }

        private void SitchNF_Click(object sender, RoutedEventArgs e)
        {
            // ( (FolderTreeInf)treeView2.SelectedItem).LibInfo.Avtor.Add(new LibPerson());
            System.Windows.Controls.Button ClickButton = (System.Windows.Controls.Button)sender;
            LibPerson pers = (LibPerson)ClickButton.Tag;
            string tmpstr = pers.Last;
            pers.Last= pers.First;
            pers.First = tmpstr;
        }
        private void DelAvtor_Click(object sender, RoutedEventArgs e)
        {
            //((FolderTreeInf)treeView2.SelectedItem).LibInfo.Avtor.Remove((LibPerson)listAvtor.SelectedItem);
            System.Windows.Controls.Button ClickButton = (System.Windows.Controls.Button)sender;
            LibPerson pers = (LibPerson)ClickButton.Tag;
            ObservableCollection<LibPerson> listpers = (ObservableCollection<LibPerson>)ClickButton.DataContext;
            listpers.Remove(pers);
        }

        private void treeSeq_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void addSeq_Click(object sender, RoutedEventArgs e)
        {
           // ((FolderTreeInf)treeView2.SelectedItem).LibInfo.BookSequence.Add(new LibSequence(null));
            System.Windows.Controls.Button ClickButton = (System.Windows.Controls.Button)sender;
            //LibSequence pers = (LibSequence)ClickButton.Tag==null?;
            ObservableCollection<LibSequence> listseq = ((LibSequence)ClickButton.Tag==null ? (ObservableCollection<LibSequence>)ClickButton.DataContext:((LibSequence)ClickButton.Tag).SubSequence ) ;
            LibSequence seq = new LibSequence((LibSequence)ClickButton.Tag);
            listseq.Add(seq);
        }

        //private void addSubSeq_Click(object sender, RoutedEventArgs e)
        //{
        //    ((LibSequence)treeSeq.SelectedItem).SubSequence.Add(new LibSequence(((LibSequence)treeSeq.SelectedItem)));
        //}

        private void delSeq_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button ClickButton = (System.Windows.Controls.Button)sender;
            if ((LibSequence)ClickButton.Tag != null)
            {
                
                ObservableCollection<LibSequence> listseq = (((LibSequence)ClickButton.Tag).Parent == null ? (ObservableCollection<LibSequence>)ClickButton.DataContext : ((LibSequence)ClickButton.Tag).Parent.SubSequence  );
                //LibSequence seq = new LibSequence((LibSequence)ClickButton.Tag);
                listseq.Remove((LibSequence)ClickButton.Tag);
               // listseq.Add((LibSequence)ClickButton.Tag);
            }
            //if (((LibSequence)treeSeq.SelectedItem).Parent == null)
            //    ((FolderTreeInf)treeView2.SelectedItem).LibInfo.BookSequence.Remove(((LibSequence)treeSeq.SelectedItem));
            //else
            //    ((LibSequence)treeSeq.SelectedItem).Parent.SubSequence.Remove(((LibSequence)treeSeq.SelectedItem));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GenreTree GenreTreeWin ;
            if (MultiSel == null)
                GenreTreeWin = new GenreTree(((FolderTreeInf)treeView2.SelectedItem).LibInfo.Book._Theme);
            else
                GenreTreeWin = new GenreTree(MultiSel.LibInfo.Book._Theme);
            GenreTreeWin.ShowDialog();
            
        }

        private void DelPic_Click(object sender, RoutedEventArgs e)
        {
            if (MultiSel == null)
                ((FolderTreeInf)treeView2.SelectedItem).LibInfo.Book.Cover.Clear();
            else
                MultiSel.LibInfo.Book.Cover.Clear();
        }

        private void AddPic_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog opf = new System.Windows.Forms.OpenFileDialog();
            opf.Multiselect = false;
            opf.InitialDirectory = System.IO.Path.GetDirectoryName(((FolderTreeInf)treeView2.SelectedItem).FPath);
            opf.Filter = "All files (*.*)|*.*";
            if (opf.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            {
               // ((FolderTreeInf)treeView2.SelectedItem).LibInfo.Book.Cover.FillFromFile(opf.FileName);
                if (MultiSel == null)
                    ((FolderTreeInf)treeView2.SelectedItem).FillCoverNextPage(opf.FileName);
                else
                    MultiSel.FillCoverNextPage(opf.FileName);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox  ClickButton = (CheckBox)sender;
            FolderTreeInf CheckedItem =(FolderTreeInf)ClickButton.Tag;
            CheckedItem.FillLibInf(this.Option.Mask);
            if (MultiSel==null)
            {
                MultiSel=new FolderTreeInf() ;
                stackPanel1.DataContext=MultiSel ;
            }
            MultiSel.Node.Add(CheckedItem);
            MultiSel.LibInfo.FileInf.Content.Add(new LibContentInf(CheckedItem.FName));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox ClickButton = (CheckBox)sender;
            FolderTreeInf CheckedItem = (FolderTreeInf)ClickButton.Tag;
            MultiSel.Node.Remove(CheckedItem);

            ObservableCollection<LibContentInf> delcol=MultiSel.LibInfo.FileInf.Content;
            LibContentInf LibCon2Del = delcol.First(dc => dc.FileName == CheckedItem.FName);
            delcol.Remove(LibCon2Del);
            if (MultiSel.Node.Count == 0)
            {
                Binding myBinding = new Binding();
                myBinding.ElementName ="treeView2";
                myBinding.Path = new PropertyPath("SelectedItem");

                stackPanel1.SetBinding(StackPanel.DataContextProperty, myBinding);
               // stackPanel1.DataContext = myBinding; //treeView2.SelectedItem;
                MultiSel = null;
            }
        }

        private void FillBook_Click(object sender, RoutedEventArgs e)
        {
            if (MultiSel == null)
            {
                FolderTreeInf item = (FolderTreeInf)treeView2.SelectedItem;
                //  FolderTreeInf itemold = (FolderTreeInf)e.OldValue;
                if (item != null)
                {
                    item.SaveLibInf(Option);
                    //item.SaveLibInf(this.Option.Mask);
                }
                //if (itemold != null)
                //{
                //    itemold.SaveLibInf();
                //       //FillLibInf(this.Option.Mask);
                //}
            }
            else 
            {
                MultiSel.MultiSave(Option);
                
            }
            string FnodePath=((FolderTreeInf)treeView1.SelectedItem).FPath;
            FolderTreeInf.ReReadListFields(nodes2, FnodePath);
           
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                FolderTreeInf item = (FolderTreeInf)treeView2.SelectedItem;
                if (MultiSel == null)
                    item.FillCoverNextPage();
                else
                    MultiSel.FillCoverNextPage();
            }
        }

        private void GetFromText_Click(object sender, RoutedEventArgs e)
        {
            

            if (MultiSel == null)
            {
                FolderTreeInf TextFile = (FolderTreeInf)treeView2.SelectedItem;
                string PathTextFile = TextFile.FPath;
                string textFType = System.IO.Path.GetExtension(PathTextFile);
                string TextFromBook=string.Empty;
                if (textFType.ToLower() == ".djvu" || textFType.ToLower() == ".djv" || textFType.ToLower() == ".pdf" )
                {
                    if (textFType.ToLower() == ".pdf")
                    {

                        PdfDocument document = new PdfDocument(PathTextFile);
                        for (int i = 0; i < 10; i++)
                        {
                            PdfPage page = document.GetPage(i);
                            TextFromBook += page.GetPdfText();
                            page.Dispose();
                        }
                        document.Dispose();
                    }
                    if (textFType.ToLower() == ".djvu" || textFType.ToLower() == ".djv")
                    {
                        DjvuDocument document = new DjvuDocument(PathTextFile);
                        for (int i = 0; i < 10; i++)
                        {
                            var page = document.Pages[i];
                            TextFromBook += page.Text;
                          }
 
                    }
                    if (!string.IsNullOrWhiteSpace(TextFromBook))
                    {
                        FieldFromText FieldFromTextWin;
                        FieldFromTextWin = new FieldFromText(TextFile.LibInfo, TextFromBook);
                        FieldFromTextWin.ShowDialog();
                    }
                }
            }
        }

        private void GetFromGoogle_Click(object sender, RoutedEventArgs e)
        {
            if (MultiSel == null)
            {
                FolderTreeInf TextFile = (FolderTreeInf)treeView2.SelectedItem;
                GBooksSearch gBooksSearchWin = new GBooksSearch(TextFile.LibInfo);
                gBooksSearchWin.ShowDialog();
            }


            //Template="{DynamicResource TreeViewControlTemplate1}" ItemTemplate="{DynamicResource DataTemplate1}"
            // <ColumnDefinition Width="Auto" MinWidth="19" />
        }
    }

}

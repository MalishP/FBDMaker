using System;
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
using System.Windows.Shapes;


namespace FBDMaker
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class GenreTree : Window
    {
        public GenreList Themes;
        public GenreTree(GenreList Gen)
        {
            Themes = Gen;
            InitializeComponent();
            Themes.ResetExpand();
            treeView1.ItemsSource = Themes.Glist ;
            
        }

        //private void ExpandChaild(TreeViewItem tri)
        //{
        //    foreach (GenreNode itm in tri.Items)
        //    {
        //        try
        //        {
        //            DependencyObject dObject = tri.ItemContainerGenerator.ContainerFromItem(itm);
        //            DependencyObject dObject1 = treeView1.ItemContainerGenerator.ContainerFromItem(itm);

        //            ((TreeViewItem)dObject).IsExpanded = itm.IsChekedChaild;
        //            ExpandChaild((TreeViewItem)dObject);
        //        }
        //        catch { }
        //    }
        //}

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Themes.ChangeAccept();
            this.DialogResult = true;
        }

       

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Themes.ChangeCancel();
        }

        //private void Window_Activated(object sender, EventArgs e)
        //{
        //    foreach (GenreNode itm in treeView1.Items)
        //    {
        //        //try
        //        //{
        //        DependencyObject dObject = treeView1.ItemContainerGenerator.ContainerFromItem(itm);

        //        ((TreeViewItem)dObject).IsExpanded  = itm.IsChekedChaild;
        //        ExpandChaild((TreeViewItem)dObject);
        //        //}
        //        //catch { }

        //    }
        //}
    }
}

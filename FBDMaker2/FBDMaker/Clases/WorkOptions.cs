using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace FBDMaker
{
    public enum MultySelOperationEnum
    {
        CopyField,
        MakeArch
    };
    public enum MakeBookArchEnum
    {
        MakeBookArch_Yes,
        MakeBookArch_No
    };
    public enum InsertFBDArchEnum
    {
        InsertFBDArch_Yes,
        InsertFBDArch_No
    };
    public enum FolderRootAddArchEnum
    {
        FolderRootAddArch_Yes,
        FolderRootAddArch_No
    };
    public enum DeleteAfterArchEnum
    {
        DeleteAfterArch_Yes,
        DeleteAfterArch_No
    };

    public enum RenameFilesEnum
    {
        RenameFiles_Yes,
        RenameFiles_No
    };
    public class WorkOptions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public StringMaskParser Mask  { get; private set; }
        public MultySelOperationEnum MultySelOperation { get; set; }

        public MakeBookArchEnum MakeBookArch{get;set;}
        public InsertFBDArchEnum InsertFBDArch { get; set; }

        public FolderRootAddArchEnum FolderRootAddArch { get; set; }
        public DeleteAfterArchEnum DeleteAfterArch { get; set; }

        public RenameFilesEnum RenameFiles { get; set; }
        public WorkOptions ()
        {
            Mask = new StringMaskParser("%av - %tl");
            MultySelOperation = MultySelOperationEnum.CopyField;
            MakeBookArch = MakeBookArchEnum.MakeBookArch_Yes;
            InsertFBDArch = InsertFBDArchEnum.InsertFBDArch_Yes;
        }
        public bool MultySelOperation_Copy
        {
            get { return MultySelOperation == MultySelOperationEnum.CopyField; }
            set
            {
                MultySelOperation = MultySelOperationEnum.CopyField;
                NotifyPropertyChanged("MultySelOperation_Copy");
                NotifyPropertyChanged("MultySelOperation_Make");
            }
        }
        public bool MultySelOperation_Make
        {
            get { return MultySelOperation == MultySelOperationEnum.MakeArch; }
            set
            {
                MultySelOperation = MultySelOperationEnum.MakeArch;
                NotifyPropertyChanged("MultySelOperation_Copy");
                NotifyPropertyChanged("MultySelOperation_Make");
            }
        }
        public bool MakeArch_Yes
        {
            get { return MakeBookArch==MakeBookArchEnum.MakeBookArch_Yes;}
            set {
                MakeBookArch = MakeBookArchEnum.MakeBookArch_Yes;
                NotifyPropertyChanged("MakeArch_Yes");
                NotifyPropertyChanged("MakeArch_No");
            }
        }
        public bool MakeArch_No
        {
            get { return MakeBookArch == MakeBookArchEnum.MakeBookArch_No; }
            set {
                MakeBookArch = MakeBookArchEnum.MakeBookArch_No;
                NotifyPropertyChanged("MakeArch_Yes");
                NotifyPropertyChanged("MakeArch_No");
            }
        }

        public bool InsertFBDArch_Yes
        {
            get { return InsertFBDArch == InsertFBDArchEnum.InsertFBDArch_Yes; }
            set {
                InsertFBDArch = InsertFBDArchEnum.InsertFBDArch_Yes;
                NotifyPropertyChanged("InsertFBDArch_Yes");
                NotifyPropertyChanged("InsertFBDArch_No");
            }
        }
        public bool InsertFBDArch_No
        {
            get { return InsertFBDArch == InsertFBDArchEnum.InsertFBDArch_No; }
            set {
                InsertFBDArch = InsertFBDArchEnum.InsertFBDArch_No;
                NotifyPropertyChanged("InsertFBDArch_Yes");
                NotifyPropertyChanged("InsertFBDArch_No");
            }
        }

        public bool FolderRootAddArch_Yes
        {
            get { return FolderRootAddArch==FolderRootAddArchEnum.FolderRootAddArch_Yes;}
            set{
                FolderRootAddArch = FolderRootAddArchEnum.FolderRootAddArch_Yes;
                NotifyPropertyChanged("FolderRootAddArch_Yes");
                NotifyPropertyChanged("FolderRootAddArch_No");
            }
        }
        public bool FolderRootAddArch_No
        {
            get { return FolderRootAddArch==FolderRootAddArchEnum.FolderRootAddArch_No;}
            set{
                FolderRootAddArch = FolderRootAddArchEnum.FolderRootAddArch_No;
                NotifyPropertyChanged("FolderRootAddArch_Yes");
                NotifyPropertyChanged("FolderRootAddArch_No");
            }
        }

        public bool DeleteAfterArch_Yes
        {
            get { return DeleteAfterArch == DeleteAfterArchEnum.DeleteAfterArch_Yes; }
            set
            {
                DeleteAfterArch = DeleteAfterArchEnum.DeleteAfterArch_Yes;
                NotifyPropertyChanged("DeleteAfterArch_Yes");
                NotifyPropertyChanged("DeleteAfterArch_No");
            }
        }
        public bool DeleteAfterArch_No
        {
            get { return DeleteAfterArch == DeleteAfterArchEnum.DeleteAfterArch_No; }
            set
            {
                DeleteAfterArch = DeleteAfterArchEnum.DeleteAfterArch_No;
                NotifyPropertyChanged("DeleteAfterArch_Yes");
                NotifyPropertyChanged("DeleteAfterArch_No");
            }
        }
 public bool RenameFiles_Yes
        {
            get { return RenameFiles == RenameFilesEnum.RenameFiles_Yes; }
            set
            {
                RenameFiles = RenameFilesEnum.RenameFiles_Yes;
                NotifyPropertyChanged("RenameFiles_Yes");
                NotifyPropertyChanged("RenameFiles_No");
            }
        }
        public bool RenameFiles_No
        {
            get { return RenameFiles == RenameFilesEnum.RenameFiles_No; }
            set
            {
                RenameFiles = RenameFilesEnum.RenameFiles_No;
                NotifyPropertyChanged("RenameFiles_Yes");
                NotifyPropertyChanged("RenameFiles_No");
            }
        }
    }
}

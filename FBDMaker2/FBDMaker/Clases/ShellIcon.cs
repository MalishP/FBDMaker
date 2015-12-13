using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;

namespace FBDMaker
{
    /// <summary>
    /// Summary description for ShellIcon.
    /// </summary>
    /// <summary>
    /// Summary description for ShellIcon.  Get a small or large Icon with an easy C# function call
    /// that returns a 32x32 or 16x16 System.Drawing.Icon depending on which function you call
    /// either GetSmallIcon(string fileName) or GetLargeIcon(string fileName)
    /// </summary>
    public class ShellIcon
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };


        class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
            public const uint SHGFI_SMALLICON = 0x1; // 'Small icon
            public const uint SHGFI_SHELLICONSIZE = 0x4; // 'shell size icon

            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        }


        public static Icon GetSmallIcon(string fileName)
        {
            IntPtr hImgSmall; //the handle to the system image list
            SHFILEINFO shinfo = new SHFILEINFO();


            //Use this to get the small Icon
            hImgSmall = Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);


            //The icon is returned in the hIcon member of the shinfo struct
            return System.Drawing.Icon.FromHandle(shinfo.hIcon);
        }


        public static Icon GetLargeIcon(string fileName)
        {
            IntPtr hImgLarge; //the handle to the system image list
            SHFILEINFO shinfo = new SHFILEINFO();


            //Use this to get the large Icon
            hImgLarge = Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON); 


            //The icon is returned in the hIcon member of the shinfo struct
            return System.Drawing.Icon.FromHandle(shinfo.hIcon);
        }

        public static ImageSource ImageSourceSmallIcon(string fileName)
        {
            Icon icon = GetSmallIcon(fileName);
            Bitmap bmp = icon.ToBitmap();

            MemoryStream strm = new MemoryStream();
            bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            strm.Seek(0, SeekOrigin.Begin);
            bmpImage.StreamSource = strm;
            bmpImage.EndInit();

            return bmpImage;
        }
        public static ImageSource ImageSourceLargeIcon(string fileName)
        {
            Icon icon = GetLargeIcon(fileName);
            Bitmap bmp = icon.ToBitmap();

            MemoryStream strm = new MemoryStream();
            bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            strm.Seek(0, SeekOrigin.Begin);
            bmpImage.StreamSource = strm;
            bmpImage.EndInit();

            return bmpImage;
        }
            }
}

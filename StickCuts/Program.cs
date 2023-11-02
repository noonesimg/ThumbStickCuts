using DXNET.XInput;
using StickCuts;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using ThumbStickCuts.Util;
using WindowsInput.Native;
using static System.Net.Mime.MediaTypeNames;


namespace ThumbStickCuts
{
    public class Program
    {
        

        [STAThread]
        public static void Main()
        {
            Win32.HideConsole();
            var app = new System.Windows.Application();
            MainWindow win = new MainWindow();
            app.Run(win);
        }
    }
}




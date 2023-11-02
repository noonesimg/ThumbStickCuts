using DXNET.XInput;
using StickCuts;
using StickCuts.Actions;
using StickCuts.Config;
using StickCuts.Layouts;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using WindowsInput.Native;
using static System.Net.Mime.MediaTypeNames;


namespace StickCuts
{
    public class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;

        [STAThread]
        public static void Main()
        {
            var handle = FindWindow(null, Console.Title);
            ShowWindow(handle, SW_HIDE);

            var app = new System.Windows.Application();
            MainWindow win = new MainWindow();
            app.Run(win);
        }
    }
}


    

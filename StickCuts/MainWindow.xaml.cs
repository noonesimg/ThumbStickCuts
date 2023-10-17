using StickCuts.Actions;
using StickCuts.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StickCuts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InputManager input;
        Dictionary<ThumbZone, ContentControl> controls;

        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += OnRendering;
            Loaded += OnLoaded;
            Topmost = true;
            ShowInTaskbar = false;
        }

        private void OnLoaded(object? sender, EventArgs e)
        {
            Left = SystemParameters.VirtualScreenWidth / 2 - Width / 2;
            Top = SystemParameters.VirtualScreenHeight - Height - 50;
            controls = new Dictionary<ThumbZone, ContentControl>()
            {

                { ThumbZone.TopLeft, TopLeftControl },
                { ThumbZone.Top, TopControl },
                { ThumbZone.TopRight, TopRightControl },
                
                { ThumbZone.Left, LeftControl },
                { ThumbZone.Right, RightControl },

                { ThumbZone.BottomLeft, BottomLeftControl },
                { ThumbZone.Bottom, BottomControl },
                { ThumbZone.BottomRight, BottomRightControl },
            };

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            SetWindowExTransparent(hwnd);

            input = new InputManager();
            input.OnLayoutChange += UpdateControls;
            input.OnActionSelected += SelectAction;
            input.LoadLayouts();
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            if (input != null)
                input.Update();
        }

        private void SelectAction(object? sender, ThumbZone zone)
        {
            if (zone == ThumbZone.Center)
            {
                foreach (var c in controls.Values)
                {
                    c.SetCurrentValue(ForegroundProperty, Brushes.Black);
                }
            }
            else
            {
                controls[zone].SetCurrentValue(ForegroundProperty, Brushes.Red);
            }
        }

        private void UpdateControls(object? sender, Dictionary<ThumbZone, IAction?>? currentActions)
        {
            if (currentActions == null)
                return;

            foreach(var c in controls.Values)
            {
                c.Content = null;
            }

            foreach(var kv in currentActions)
            {
                var control = controls[kv.Key];
                var action = kv.Value;
                {
                    var text = action?.Icon;
                    if (int.TryParse(action?.Icon, NumberStyles.HexNumber, null, out var unicodeInt))
                    {
                        text = Char.ConvertFromUtf32(unicodeInt);
                    }
                    control.Content = new TextBlock
                    {
                        Text = text,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 20
                    };
                }
            }
        }

        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }
}

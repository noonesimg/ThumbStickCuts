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
using ThumbStickCuts.Actions;
using ThumbStickCuts.Input;
using ThumbStickCuts.Util;

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
            Win32.FixWindowStyle(hwnd);
            
            try
            {
                input = new InputManager();
                input.OnLayoutChange += UpdateControls;
                input.OnActionSelected += SelectAction;

                input.ToggleHidden += (s, e) => {
                    if (IsVisible)
                        Hide();
                    else
                        Show();
                };
                input.LoadLayouts();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Shutdown();
            }
            
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
                    
                    control.Content = new TextBlock
                    {
                        Text = action?.Icon, 
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 25
                    };
                }
            }
        }
    }
}

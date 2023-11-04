using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using ThumbStickCuts.Actions;
using ThumbStickCuts.Config;
using ThumbStickCuts.Input;
using ThumbStickCuts.Util;
using MessageBox = System.Windows.Forms.MessageBox;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

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
                input.OnWindowStyleChanged += UpdateWindow;
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
                System.Windows.Application.Current.Shutdown();
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
                    c.Foreground = Foreground;
                }
            }
            else
            {
                controls[zone].Foreground = System.Windows.Media.Brushes.Red;
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
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        FontSize = 25
                    };
                }
            }
        }

        private SolidColorBrush HexToBrush(string hexColor, float opacity)
        {
            Color color = (Color)ColorConverter.ConvertFromString(hexColor);
            color.A = (byte)(opacity * 255);
            return new SolidColorBrush(color);
        }

        private void UpdateWindow(object? sender, WindowStyleDto style)
        {
            var currentScreen = Screen.AllScreens[style.ScreenIndex];

            Width = style.Width;
            Height = style.Height;
            Left = currentScreen.Bounds.Left + style.Left;
            Top = currentScreen.Bounds.Top + currentScreen.Bounds.Height - style.Height - style.Bottom;

            Background = HexToBrush(style.BgColor, style.BgOpacity);
            Foreground = HexToBrush(style.FgColor, 1.0f);
            CenterIcon.Text = style.Icon;
            SelectAction(this, ThumbZone.Center);
        }
    }
}

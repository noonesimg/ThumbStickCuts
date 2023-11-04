using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using ThumbStickCuts.Input;
using ThumbStickCuts.Layouts;

namespace ThumbStickCuts.Config
{
    public class WindowStyleDto
    {
        public string Icon { set; get; } = String.Empty;
        public float Width { set; get; } = 100;
        public float Height { set; get; } = 100;
        public float Bottom { set; get; } = 10;
        public float Left { set; get; } = 10;
        public int ScreenIndex { set; get; } = 0;
        public string BgColor { set; get; } = "#FFFFFF";
        public string FgColor { set; get; } = "#000000";
        public float BgOpacity { set; get; } = 0.2f;

    }
    public class LayoutDto
    {
        public string? Name { set; get; }
        public WindowStyleDto Window { set; get; } = new WindowStyleDto();

        public Dictionary<ThumbZone, Dictionary<ThumbZone, ActionDto?>>? Actions { get; set; }


        public Layout? ToLayout()
        {
            if (Actions == null)
                return null;

            Layout l = new Layout();
            l.Name = Name;
            l.Actions = Actions.ToDictionary(
                outerKV => outerKV.Key,
                outerKV => outerKV.Value.ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value?.ToAction() ?? null
                )
            );
            l.Window = Window;

            return l;
        }
    }
}

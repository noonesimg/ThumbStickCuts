using DXNET.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThumbStickCuts.Actions;
using ThumbStickCuts.Config;
using ThumbStickCuts.Input;

namespace ThumbStickCuts.Layouts
{
    public class Layout
    {
        public string? Name { set; get; }
        public Dictionary<ThumbZone, Dictionary<ThumbZone, IAction?>>? Actions { set; get; }

        public bool TryGetActions(ThumbZone zone, out Dictionary<ThumbZone, IAction?>? actions)
        {
            actions = null;
            if (Actions == null)
                return false;

            if (Actions.Count == 0)
                return false;

            else if (Actions.ContainsKey(zone))
            {
                actions = Actions[zone];
                return true;
            }
            else return false;
        }

        public WindowStyleDto Window { set; get; } = new WindowStyleDto();
    }
}

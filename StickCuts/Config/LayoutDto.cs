using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThumbStickCuts.Input;
using ThumbStickCuts.Layouts;

namespace ThumbStickCuts.Config
{
    public class LayoutDto
    {
        public string? Name { set; get; }
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

            return l;
        }
    }
}

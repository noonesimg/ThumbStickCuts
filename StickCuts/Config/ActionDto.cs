using StickCuts.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Provider;
using WindowsInput.Native;

namespace StickCuts.Config
{
    public class KeyCombo
    {
        public VirtualKeyCode Key { set; get; }
        public List<VirtualKeyCode> Modifiers { set; get; } = new();
    }

    public class ActionDto
    {
        public string? Icon { set; get; }
        public string? Action { set; get; }

        public List<KeyCombo>? Keys { set; get; }
        
        public IAction? ToAction()
        {
            if (string.IsNullOrEmpty(Action))
                return null;

            if (Action.ToLower() == "reload")
            {
                return new SpecialAction()
                {
                    Icon = Icon ?? string.Empty,
                    Type = ActionTypes.RELOAD
                };
            }
            else
            {
                return new KeyAction()
                {
                    Icon = Icon ?? Action,
                    Keys = ShortCutParser.ParseShortCut(Action)
                };
            }
        }
    }
}

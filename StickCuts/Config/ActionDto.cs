using StickCuts.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace StickCuts.Config
{
    public class KeyStrokeDto
    {
        public string? Key { get; set; }
        public bool? Shift { set; get; }
        public bool? Ctrl { set; get; }
        public bool? Alt { set; get; }
    }
    public class ActionDto
    {
        public ActionTypes Type { set; get; }
        public string? Icon { set; get; }

        // for keyboard shortcut
        public List<KeyStrokeDto>? Keys { set; get; }
        

        // for starting file
        public string? FilePath { set; get; }
        public string? Arguments { set; get; }

        public IAction? ToAction()
        {
            switch (Type)
            {
                case ActionTypes.SHORTCUT:
                    {
                        if (Keys == null || Keys.Count == 0)
                            return null;

                        return KeyAction.FromDto(Keys, Icon);
                    }
                case ActionTypes.EXE:
                    {
                        if (string.IsNullOrEmpty(FilePath))
                            return null;

                        return new FileAction(FilePath, Arguments, Icon);
                    }
                case ActionTypes.Reload:
                    {
                        return new SpecialAction()
                        {
                            Icon = Icon ?? string.Empty,
                            Type = Type
                        };
                    }
            }
            return null;
        }
    }
}

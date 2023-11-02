using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ThumbStickCuts.Config;
using WindowsInput;
using WindowsInput.Native;

namespace ThumbStickCuts.Actions
{
    internal class KeyAction : IAction
    {
        public ActionTypes Type => ActionTypes.SHORTCUT;
        public string Icon { set; get; } = string.Empty;
        public List<KeyCombo> Keys { get; set; } = new();

        public void Perform()
        {
            foreach (var k in Keys)
            {
                new InputSimulator().Keyboard.ModifiedKeyStroke(
                    k.Modifiers,
                    new[] { k.Key }
                );
            }

        }
    }
}

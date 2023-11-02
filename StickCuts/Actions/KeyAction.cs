using StickCuts.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;

namespace StickCuts.Actions
{
    class KeyStroke
    {
        public VirtualKeyCode Key;
        public VirtualKeyCode[] Modifiers = Array.Empty<VirtualKeyCode>();

        public static KeyStroke? FromDto(KeyStrokeDto dto)
        {
            if (Enum.TryParse<VirtualKeyCode>(dto.Key, out var key))
            {
                var modifiers = new List<VirtualKeyCode>();
                if (dto.Ctrl.GetValueOrDefault())
                    modifiers.Add(VirtualKeyCode.CONTROL);
                if (dto.Alt.GetValueOrDefault())
                    modifiers.Add(VirtualKeyCode.MENU);
                if (dto.Shift.GetValueOrDefault())
                    modifiers.Add(VirtualKeyCode.SHIFT);

                return new KeyStroke()
                {
                    Key = key,
                    Modifiers = modifiers.ToArray()
                };
            }
            else return null;
        }
    }

    internal class KeyAction : IAction
    {
        public ActionTypes Type => ActionTypes.SHORTCUT;
        public string Icon { private set; get; } = string.Empty;

        public List<KeyStroke?> Keys { get; set; }
        
        public KeyAction(List<KeyStrokeDto> keys, string? icon)
        {
            Keys = keys.Select(x => KeyStroke.FromDto(x)).ToList();
            if (icon != null)
                Icon = icon;
        }

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

        public static KeyAction FromDto(List<KeyStrokeDto> keys, string? icon = null)
        {
            return new KeyAction(keys, icon);
        }


        [DllImport("user32.dll")]
        static extern short VkKeyScan(char ch);
    }
}

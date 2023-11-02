using StickCuts.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsInput.Native;

namespace StickCuts
{
    internal class ShortCutParser
    {
        [DllImport("user32.dll")]
        static extern short VkKeyScan(char ch);

        public static Dictionary<string, string> Replacements = new()
        {
            { "Ctrl", "LCONTROL" },
            { "Alt", "LMENU" },
            { "Esc", "ESCAPE" },
            { "Enter", "RETURN" }
        };

        public static List<KeyCombo> ParseShortCut(string input)
        {
            if (string.IsNullOrEmpty(input)) {
                throw new ArgumentNullException("empty shortcut");
            }

            List<KeyCombo> result = new();

            var items = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                try
                {
                    var shortcut = new KeyCombo();
                    var sKeys = item.Split('+', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var k in sKeys)
                    {
                        if (k == sKeys.Last())
                        {
                            if (k.Length > 1 && Enum.TryParse<VirtualKeyCode>(k, true, out var key))
                                shortcut.Key = key;
                            else 
                                shortcut.Key = (VirtualKeyCode)VkKeyScan(k[0]);
                        }
                        else
                        {
                            var modifierString = k;
                            if (Replacements.ContainsKey(k))
                            {
                                modifierString = Replacements[k];
                            }
                            var vkCode = Enum.Parse<VirtualKeyCode>(modifierString, true);
                            shortcut.Modifiers.Add(vkCode);
                        }
                    }
                    result.Add(shortcut);
                }
                catch (Exception) 
                {
                    throw new ArgumentException($"couldn't parse shortcut: {item}");
                }
            }

            return result;
        }
    }
}

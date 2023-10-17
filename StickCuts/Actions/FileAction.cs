using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickCuts.Actions
{
    internal class FileAction : IAction
    {
        public ActionTypes Type => ActionTypes.EXE;

        public string Icon { set; get; } = string.Empty;
        public string? FilePath { set; get; }
        public string? Arguments { set; get; }

        public FileAction(string filePath, string? arguments = null, string? icon = null)
        {
            FilePath = filePath;
            Arguments = arguments == null ? string.Empty : arguments;
            Icon = icon == null ? string.Empty : icon;
        }

        public void Perform()
        {
            if (string.IsNullOrEmpty(FilePath))
                return;

            if (!File.Exists(FilePath))
                return;

            Process.Start(new ProcessStartInfo()
            {
                FileName = FilePath,
                Arguments = Arguments
            });
        }
    }
}

using StickCuts.Actions;
using StickCuts.Layouts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StickCuts.Config
{
    public class ConfigHandler
    {
        private readonly string dirPath = "configs";

        public ConfigHandler()
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }

        public List<Layout> LoadLayouts()
        {
            var files = Directory.EnumerateFiles(dirPath);
            List<Layout> layouts = new();
            foreach (var file in files)
            {
                if (TryLoadLayout(file, out var l) && l != null)
                {
                    if (string.IsNullOrEmpty(l.Name))
                        l.Name = file.Replace(".json", "");

                    layouts.Add(l);
                }
            }
            return layouts;
        }

        public bool TryLoadLayout(string fileName, out Layout? l)
        {
            l = null;
            var json = File.ReadAllText(fileName);
            var opt = new JsonSerializerOptions()
            {
                Converters = { new JsonStringEnumConverter() }
            };
            var dto = JsonSerializer.Deserialize<LayoutDto>(json, opt);
            if (dto == null)
                return false;

            l = dto.ToLayout();
            return l != null;
        }
    }
}

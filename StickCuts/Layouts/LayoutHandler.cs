using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ThumbStickCuts.Layouts
{
    public class LayoutHandler
    {
        public List<Layout> Layouts;
        private int Index = 0;

        public LayoutHandler()
        {
            Layouts = new List<Layout>();
        }

        public Layout GetNext()
        {
            if (Layouts.Count == 0)
                throw new Exception("No layouts");

            Index++;
            Index %= Layouts.Count;
            return Layouts[Index];
        }

        public Layout GetPrevious()
        {
            if (Layouts.Count == 0)
                throw new Exception("No layouts");

            if (Index == 0)
            {
                Index = Layouts.Count - 1;
            }
            else
            {
                Index--;
                Index %= Layouts.Count;
            }

            return Layouts[Index];
        }

        public Layout GetDefaultLayout()
        {
            if (Layouts.Count == 0)
                throw new Exception("No layouts");

            return Layouts[Index];
        }
    }
}

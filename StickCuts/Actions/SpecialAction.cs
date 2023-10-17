using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickCuts.Actions
{
    internal class SpecialAction : IAction
    {
        public ActionTypes Type { set; get; }
        public string Icon { set; get; } = string.Empty;

        public void Perform()
        {
            throw new NotImplementedException();
        }
    }
}

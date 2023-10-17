using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickCuts.Actions
{
    public interface IAction
    {
        ActionTypes Type { get; }
        string Icon { get; }
        void Perform();
    }
}

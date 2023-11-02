using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace StickCuts.Actions
{
    public enum ActionTypes
    {
        None,
        SHORTCUT,
        EXE,
        RELOAD
    }
}

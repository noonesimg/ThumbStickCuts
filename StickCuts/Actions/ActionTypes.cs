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

    //public class ActionTypeConverter : JsonConverter<ActionTypes>
    //{
    //    public override ActionTypes Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        return Enum.Parse<ActionTypes>(reader.GetString() ?? ActionTypes.None.ToString(), true);
    //    }

    //    public override void Write(Utf8JsonWriter writer, ActionTypes value, JsonSerializerOptions options)
    //    {
    //        writer.WriteStringValue(value.ToString());
    //    }
    //}
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Setas.Helpers
{
    static class Predictions
    {
        public static int TagToItemId(string tag)
        {
            var id = JArray.Parse("["+ tag + "]")[0].Value<int>();

            return id;
        }
    }
}

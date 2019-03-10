using Newtonsoft.Json.Linq;
using System;

namespace Setas.Helpers
{
    public static class Predictions
    {
        public static int TagToItemId(string tag)
        {
            int id = default(int);
            try
            {
                id = int.Parse(JArray.Parse("[" + tag + "]")[0].Value<string>());
            }
            catch (Exception ex)
            { }

            return id;
        }
    }
}

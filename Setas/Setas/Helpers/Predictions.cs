using Newtonsoft.Json.Linq;
using System;

namespace Setas.Helpers
{
    public static class Predictions
    {
        public static int TagToItemId(string tag)
        {
            int id = 0;
            try
            {
                id = JArray.Parse("[" + tag + "]")[0].Value<int>();
            }
            catch (Exception ex)
            { }

            return id;
        }
    }
}

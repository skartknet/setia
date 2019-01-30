using Newtonsoft.Json.Linq;
using System;

namespace Setas.Helpers
{
    public static class Predictions
    {
        public static Guid TagToItemId(string tag)
        {
            Guid id;
            try
            {
                id = JArray.Parse("[" + tag + "]")[0].Value<Guid>();
            }
            catch (Exception ex)
            { }

            return id;
        }
    }
}

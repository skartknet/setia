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
                id = int.Parse(tag.Split(':')[0]);
            }
            catch (Exception ex)
            { }

            return id;
        }
    }
}

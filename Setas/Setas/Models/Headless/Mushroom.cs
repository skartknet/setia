using Setas.Common.Enums;
using SQLite;
using System;
using Umbraco.Headless.Client.Net.Models;

namespace Setas.Common.Models.Headless
{
    public class Mushroom : ContentBase
    {
        public string Order { get; set; }
        public string[] PopularNames { get; set; }
        public string Season { get; set; }
        public string Class { get; set; }
        public string Subclass { get; set; }
        public string[] Synonyms { get; set; }
        public string Confusion { get; set; }
        public string CookingInstructions { get; set; }
        public string CookingInterest { get; set; }
        public string Description { get; set; }
        public string Family { get; set; }
        public string Habitat { get; set; }
        public string[] Images { get; set; }
    }
}

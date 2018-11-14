using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Setas.Website.Api.Models
{
    public class Mushroom
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Order { get; set; }
        public string PopularNames { get; set; }
        public string Season { get; set; }
        public string Class { get; set; }
        public string Subclass { get; set; }
        public string Synonyms { get; set; }
        public string Confusion { get; set; }
        public string CookingInstructions { get; set; }
        public string CookingInterest { get; set; }
        public string Description { get; set; }
        public string Family { get; set; }
        public string Habitat { get; set; }
        public string[] Images { get; set; }
    }
}
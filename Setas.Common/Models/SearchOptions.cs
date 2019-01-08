using Setas.Common.Enums;

namespace Setas.Common.Models
{
    public class SearchOptions
    {
        public Edible[] Edibles { get; set; }
        public int Page { get; set; }
        public int ItemsPerPage { get; set; } = 10;
    }
}

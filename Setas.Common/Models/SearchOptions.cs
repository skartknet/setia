using Setas.Common.Enums;

namespace Setas.Common.Models
{
    public class SearchOptions
    {
        public string QueryTerm { get; set; }
        public Edible[] Edibles { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

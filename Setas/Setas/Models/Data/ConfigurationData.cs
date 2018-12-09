
using SQLite;

namespace Setas.Website.Core.Models.Data
{
    [Table("Configuration")]
    public class SiteConfigurationData
    {
        [PrimaryKey, AutoIncrement)]
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Value { get; set; }
    }
}

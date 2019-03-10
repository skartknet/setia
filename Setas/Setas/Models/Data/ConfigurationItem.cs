
using SQLite;

namespace Setas.Models.Data
{
    [Table("Configuration")]
    public class ConfigurationItem
    {
        [PrimaryKey]
        public string Alias { get; set; }
        public string Value { get; set; }
    }
}

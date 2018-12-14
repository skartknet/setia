
using SQLite;

namespace Setas.Models.Data
{
    [Table("Configuration")]
    public class ConfigurationData
    {
        [PrimaryKey]
        public string Alias { get; set; }
        public string Value { get; set; }
    }
}

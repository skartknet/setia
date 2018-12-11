
using SQLite;

namespace Setas.Models.Data
{
    [Table("Configuration")]
    public class ConfigurationData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Value { get; set; }
    }
}

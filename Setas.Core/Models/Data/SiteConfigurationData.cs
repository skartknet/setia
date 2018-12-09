using Umbraco.Core.Persistence;

namespace Setas.Website.Core.Models.Data
{
    [TableName("SetasConfiguration")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class SiteConfigurationData
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Value { get; set; }
    }
}

namespace Entity.Models
{
    public class MongoDbMappingConfiguration
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
        public ICollection<string>? Collections { get; set; }
    }
}
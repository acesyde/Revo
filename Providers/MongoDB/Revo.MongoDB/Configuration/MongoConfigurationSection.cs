using Revo.Core.Configuration;

namespace Revo.MongoDB.Configuration
{
    public class MongoConfigurationSection : IRevoConfigurationSection
    {
        public bool AutoDiscoverProjectors { get; set; }
        public bool IsActive { get; set; }
        public bool UseAsPrimaryRepository { get; set; }
        public bool UseProjections { get; set; }

        public MongoConnectionConfiguration Connection { get; set; } = null;
    }
}

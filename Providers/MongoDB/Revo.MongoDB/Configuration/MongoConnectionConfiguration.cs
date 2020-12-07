namespace Revo.MongoDB.Configuration
{
    public class MongoConnectionConfiguration
    {
        public static MongoConnectionConfiguration FromConnectionString(string connectionString, string database)
        {
            return new MongoConnectionConfiguration(connectionString, database);
        }

        private MongoConnectionConfiguration(string connectionString, string database)
        {
            ConnectionString = connectionString;
            Database = database;
        }

        public string ConnectionString { get; }
        public string Database { get; }
    }
}

using RocksDbSharp;

namespace Storage
{
    
    public class WorldStorage
    {
        
        private static readonly DbOptions Options = new DbOptions()
            .SetCreateIfMissing(true)
            .SetCompression(Compression.Lz4);
        private RocksDb _database;
        public WorldStorage(string path)
        {
            _database = RocksDb.Open(Options, path);
        }
        
        public void Put(string key, byte[] value)
        {
            _database.Put(key, value, );
        }
        
    }
}
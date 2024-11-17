using System;
using System.IO;
using RocksDbSharp;
using UnityEngine;
using Utils;

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
            var fPath = $"{Application.persistentDataPath}/DBs/{path}";
            Directory.CreateDirectory(fPath);
            _database = RocksDb.Open(Options, fPath);
        }

        public bool TryGet(Vector2Int position, out byte[] value)
        {
            value = _database.Get(BitConverter.GetBytes(position.Serialize()));
            return value != null;
        }
        
        public void Put(Vector2Int key, byte[] value)
        {
            _database.Put(BitConverter.GetBytes(key.Serialize()), value);
        }

        public void Delete(Vector2Int key)
        {
            _database.Remove(BitConverter.GetBytes(key.Serialize()));
        }

        public bool KeyExists(Vector2Int key)
        {
            return _database.Get(BitConverter.GetBytes(key.Serialize())) != null;
        }

        public void Close()
        {
            _database.Dispose();
        }
    }
}
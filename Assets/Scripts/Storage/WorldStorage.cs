using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Core;
using Network.Server;
using RocksDbSharp;
using UnityEngine;
using Utils;

namespace Storage
{
    public class WorldStorage : IDisposable
    {
        private static readonly DbOptions DatabaseOptions;

        private readonly RocksDb _database;

        static WorldStorage()
        {
            InitializeRocksDbRuntimes();
            DatabaseOptions = new DbOptions()
                .SetCreateIfMissing()
                .SetCompression(Compression.Lz4);
        }


        private static void InitializeRocksDbRuntimes()
        {
            string destinationPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "runtimes");
            if (Directory.Exists(destinationPath))
            {
                return;
            }

            string packagesPath = Path.Combine(Application.streamingAssetsPath, "Packages");
            string rocksDbFolder = Directory.GetDirectories(packagesPath)
                                            .FirstOrDefault(dir =>
                                                Regex.IsMatch(Path.GetFileName(dir), @"^RocksDB\..*$"));

            if (string.IsNullOrEmpty(rocksDbFolder))
            {
                Debug.LogError("RocksDbSharp folder not found.");
                return;
            }

            string sourceRuntimePath = Path.Combine(rocksDbFolder, "runtimes");
            if (!Directory.Exists(sourceRuntimePath))
            {
                Debug.LogError("Source runtimes folder does not exist.");
                return;
            }

            FileUtils.CopyDirectory(sourceRuntimePath, destinationPath);
        }


        public WorldStorage(string worldName)
        {
            string dataDirectory = Path.Combine(Server.Instance.ServerDirectory, "Data");
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            string worldDataPath = Path.Combine(dataDirectory, worldName);
            Directory.CreateDirectory(worldDataPath);

            _database = RocksDb.Open(DatabaseOptions, worldDataPath);

            ClearDatabase();
        }

        public void ClearDatabase()
        {
            using (var iterator = _database.NewIterator())
            {
                for (iterator.SeekToFirst(); iterator.Valid(); iterator.Next())
                {
                    _database.Remove(iterator.Key());
                }
            }
        }

        public bool TryGet(Vector2Int position, out byte[] value)
        {
            byte[] keyBytes = BitConverter.GetBytes(position.Serialize());
            value = _database.Get(keyBytes);
            return value != null;
        }


        public void Put(Vector2Int key, byte[] value)
        {
            byte[] keyBytes = BitConverter.GetBytes(key.Serialize());
            _database.Put(keyBytes, value);
        }


        public void Delete(Vector2Int key)
        {
            byte[] keyBytes = BitConverter.GetBytes(key.Serialize());
            _database.Remove(keyBytes);
        }


        public bool KeyExists(Vector2Int key)
        {
            byte[] keyBytes = BitConverter.GetBytes(key.Serialize());
            return _database.Get(keyBytes) != null;
        }

 
        public void Close()
        {
            _database.Dispose();
        }


        public void Dispose()
        {
            Close();
        }
    }
}

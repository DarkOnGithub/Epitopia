﻿using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Core;
using Network;
using Network.Server;
using QFSW.QC;
using RocksDbSharp;
using UnityEngine;
using Utils;

namespace Storage
{
    public class WorldStorage
    {
        private static BetterLogger _logger = new BetterLogger(typeof(WorldStorage));
        private static readonly DbOptions Options;

        static WorldStorage()
        {
            var destinationPath = Path.Combine($"{Path.GetDirectoryName(Application.dataPath)}", "runtimes");
            if (!Directory.Exists(destinationPath))
            {
                var rocksDbFolder = Directory.GetDirectories($"{Application.dataPath}/Packages")
                                             .FirstOrDefault(dir => Regex.IsMatch(Path.GetFileName(dir), @"^RocksDB\..*$"));
                if (rocksDbFolder != null)
                {
                    var sourcePath = Path.Combine(rocksDbFolder, "runtimes");
                    if (Directory.Exists(sourcePath))
                        FileUtils.CopyDirectory(sourcePath, destinationPath);
                    else
                        _logger.LogError("Source runtimes folder does not exist.");

                }
                else
                {
                    _logger.LogError("RocksDbSharp folder not found.");
                }
            }
            Options = new DbOptions()
                     .SetCreateIfMissing(true)
                     .SetCompression(Compression.Lz4);
        }
        private RocksDb _database;

        public WorldStorage(string name)
        {

            var dataPath = $"{Application.persistentDataPath}/{Server.Instance.Info.ServerId}/Data";
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);
         
            var fPath = $"{dataPath}/{name}";
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
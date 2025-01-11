using System.IO;
using UnityEngine;

namespace Core
{
    public static class FilesManager
    {
        public static string DataPath = Application.persistentDataPath + "/Data";
        public static string ServersPath = DataPath + "/Servers";

        static FilesManager()
        {
            CreateDirectory(DataPath);
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
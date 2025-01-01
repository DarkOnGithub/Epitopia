using System.IO;

namespace Utils
{
    public static class FileUtils
    {
        public static void CopyDirectory(string sourceDir, string destinationDir, bool overwrite = true)
        {
            Directory.CreateDirectory(destinationDir);

            foreach (var filePath in Directory.GetFiles(sourceDir))
            {
                if (Path.GetExtension(filePath) == ".meta") continue;

                var fileName = Path.GetFileName(filePath);
                var destFile = Path.Combine(destinationDir, fileName);
                File.Copy(filePath, destFile, overwrite);
            }

            foreach (var subDir in Directory.GetDirectories(sourceDir))
            {
                var dirName = Path.GetFileName(subDir);
                var destSubDir = Path.Combine(destinationDir, dirName);
                CopyDirectory(subDir, destSubDir, overwrite);
            }
        }

        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static void CloneDirectory(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir))
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                if (Path.GetExtension(file) == ".meta") continue;

                var destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                var destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
                CloneDirectory(dir, destSubDir);
            }
        }
    }
}
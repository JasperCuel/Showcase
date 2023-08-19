using Cue.Core;
using System.IO;

namespace Cue.FileManagement
{
    internal static class Deleter
    {
        internal static void Delete(string path)
        {
            FileInfo file = new(path);
            if (!file.Exists)
                return;

            File.Delete(path);
            DebugLogger.Log($"Deleted file {path}", DebugType.FileManagement);
        }

        internal static void DeleteDir(string path)
        {
            DirectoryInfo dir = new(path);
            if (!dir.Exists)
                return;

            Directory.Delete(path, true);
            DebugLogger.Log($"Deleted directory {path}", DebugType.FileManagement);
        }
    }
}
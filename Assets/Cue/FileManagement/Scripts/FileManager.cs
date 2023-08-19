using Cue.Core;
using System.Collections.Generic;
using System.IO;

namespace Cue.FileManagement
{
    public static class FileManager<T> where T : class
    {
        /// <summary>
        /// Save file locally
        /// </summary>
        /// <param name="path">Path to desired file (including file extension)</param>
        /// <param name="obj">The object to serialize</param>
        /// <param name="encrypt">Encrypt the file locally (only XML)</param>
        /// <remarks>Currently supports: .xml - .json</remarks>
        public static void Save(string path, T obj, bool encrypt = true)
        {
            if (string.IsNullOrEmpty(path) || obj == null)
            {
                DebugLogger.LogWarning($"Saving: Failed to save, values are incomplete. Path: {path} - Object: {obj}", DebugType.FileManagement);
                return;
            }
            FileInfo file = new(path);
            if (file.Extension == ".xml")
                Saver<T>.SaveXml(path, obj, encrypt);
            else if (file.Extension == ".json")
                Saver<T>.SaveJson(path, obj);
            else
                DebugLogger.LogWarning($"Saving: File extension {file.Extension} is not supported at {path}", DebugType.FileManagement);
        }

        /// <summary>
        /// Loads file locally
        /// </summary>
        /// <param name="path">Path to the file to load (including file extension)</param>
        /// <param name="decrypt">If the file is encrypted, tell the method to decrypt</param>
        /// <returns>Loaded file <typeparamref name="T"/></returns>
        /// <remarks>Currently supports: .xml - .json</remarks>
        public static T Load(string path, bool decrypt = true)
        {
            FileInfo file = new(path);
            if (!file.Exists)
            {
                DebugLogger.LogWarning($"Loading: File with path {path} doesn't exist", DebugType.FileManagement);
                return null;
            }
            T obj = null;
            if (file.Extension == ".xml")
                obj = Loader<T>.LoadXml(path, decrypt);
            else if (file.Extension == ".json")
                obj = Loader<T>.LoadJson(path);
            else
                DebugLogger.LogWarning($"Loading: File extension {file.Extension} is not supported at {path}", DebugType.FileManagement);

            if (obj != null)
                DebugLogger.Log($"Loading: Finished loading {path}", DebugType.FileManagement);
            return obj;
        }

        /// <summary>
        /// Loads a local directory
        /// </summary>
        /// <param name="path">Path to the directory to load</param>
        /// <param name="decrypt">If the files are encrypted, tell the method to decrypt</param>
        /// <returns>A list of <typeparamref name="T"/> containing the loaded data</returns>
        /// <remarks>Currently supports: .xml - .json</remarks>
        public static List<T> LoadDir(string path, bool decrypt = true)
        {
            DirectoryInfo dir = new(path);
            if (!dir.Exists)
            {
                DebugLogger.LogWarning($"Loading: Directory {path} could not be found", DebugType.FileManagement);
                return null;
            }
            return Loader<T>.LoadDir(dir, decrypt);
        }

        /// <summary>
        /// Deletes a local file with specified path
        /// </summary>
        /// <param name="path">Path to the file to delete (including file extension)</param>
        public static void Delete(string path)
        {
            if (File.Exists(path))
                Deleter.Delete(path);
            else if (Directory.Exists(path))
                Deleter.DeleteDir(path);
            else
                DebugLogger.LogWarning($"Deleting: Failed deleting file {path} as it doesn't exist", DebugType.FileManagement);
        }
    }
}
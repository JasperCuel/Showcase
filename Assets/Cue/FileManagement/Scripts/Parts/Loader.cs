using Cue.Core;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Unity.Plastic.Newtonsoft.Json;

namespace Cue.FileManagement
{
    internal static class Loader<T> where T : class
    {
        internal static T LoadJson(string path)
        {
            string data = File.ReadAllText(path);
            T obj = JsonConvert.DeserializeObject<T>(data);
            return obj;
        }

        internal static T LoadXml(string path, bool decrypt = true)
        {
            string data;

            string retrievedText = File.ReadAllText(path);
            data = decrypt ? retrievedText.Decrypt() : retrievedText;
            T obj = LoadFromXMLString(data);
            return obj;
        }

        internal static List<T> LoadDirJson(string path)
        {
            DirectoryInfo dir = new(path);
            if (!dir.Exists)
            {
                DebugLogger.LogWarning($"Directory {path} could not be found", DebugType.FileManagement);
                return null;
            }

            List<T> files = new();
            foreach (FileInfo file in dir.GetFiles())
            {
                files.Add(LoadJson(file.FullName));
            }
            DebugLogger.Log($"Succesfully loaded all files from directory {path}", DebugType.FileManagement);
            return files;
        }

        internal static List<T> LoadDir(DirectoryInfo dir, bool decrypt = true)
        {
            List<T> files = new();
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Extension == ".xml")
                    files.Add(LoadXml(file.FullName, decrypt));
                else if (file.Extension == ".json")
                    files.Add(LoadJson(file.FullName));
                else
                    DebugLogger.LogWarning($"File extenstion {file.Extension} not supported at {file.FullName}", DebugType.FileManagement);
            }
            DebugLogger.Log($"Succesfully loaded all files from directory {dir.FullName}", DebugType.FileManagement);
            return files;
        }

        private static T LoadFromXMLString(string xmlText)
        {
            using StringReader stringReader = new(xmlText);
            XmlSerializer serializer = new(typeof(T));
            return serializer.Deserialize(stringReader) as T;
        }
    }
}
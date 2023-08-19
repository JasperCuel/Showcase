using Cue.Core;
using System.IO;
using System.Xml.Serialization;
using Unity.Plastic.Newtonsoft.Json;

namespace Cue.FileManagement
{
    internal static class Saver<T> where T : class
    {
        internal static void SaveJson(string path, T obj)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string saveData = JsonConvert.SerializeObject(obj);

            WriteToFile(path, saveData);
        }

        internal static void SaveXml(string path, T obj, bool encrypt)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string saveData;

            string objXml = ToXML(obj);
            saveData = encrypt ? AESEncryption.Encrypt(objXml) : objXml;

            WriteToFile(path, saveData);
        }

        private static string ToXML(T obj)
        {
            using StringWriter stringwriter = new();
            XmlSerializer serializer = new(typeof(T));
            serializer.Serialize(stringwriter, obj);
            return stringwriter.ToString();
        }

        private static void WriteToFile(string path, string data)
        {
            File.WriteAllText(path, data);
            DebugLogger.Log($"Saving: Saved file succesfully at {path}", DebugType.FileManagement);
        }
    }
}
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem
{
    public static class Saver<T> where T : class
    {
        public static void Save(T data, string fileName)
        {
            string path = Application.persistentDataPath + $"/{fileName}.vt";
            FileStream stream = new FileStream(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static T Load(string fileName)
        {
            string path = Application.persistentDataPath + $"/{fileName}.vt";

            if (!File.Exists(path)) return null;
            
            FileStream stream = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            T savableData = formatter.Deserialize(stream) as T;
            stream.Close();
            return savableData;
        }
    }
}
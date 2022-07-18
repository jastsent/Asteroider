using System;
using System.IO;
using UnityEngine;
using Directory = UnityEngine.Windows.Directory;
using File = System.IO.File;

namespace JsonSaver
{
    public sealed class JsonSaver
    {
        public async void Save<T>(T saveObject, string path) where T : class
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            
            var json = JsonUtility.ToJson(saveObject);

            using var writer = new StreamWriter(path, false);
            await writer.WriteAsync(json);
        }
        
        public T Load<T>(string path) where T : class, new()
        {
            if (FileExist(path))
            {
                var json = File.ReadAllText(path);
                var obj = JsonUtility.FromJson<T>(json);
                return obj;
            }
            else
            {
                throw new Exception("File does not exist!");
            }
        }

        public bool FileExist(string path)
        {
            return File.Exists(path);
        }
    }
}

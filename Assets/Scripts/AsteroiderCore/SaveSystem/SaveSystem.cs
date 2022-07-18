namespace AsteroiderCore.SaveSystem
{
    public sealed class SaveSystem : ISaver
    {
        private readonly JsonSaver.JsonSaver _jsonSaver;

        public SaveSystem(JsonSaver.JsonSaver jsonSaver)
        {
            _jsonSaver = jsonSaver;
        }

        public void Save<T>(T saveObject, string path) where T : class
        {
            _jsonSaver.Save(saveObject, path);
        }

        public T Load<T>(string path) where T : class, new()
        {
            return _jsonSaver.Load<T>(path);
        }

        public bool FileExist(string path)
        {
            return _jsonSaver.FileExist(path);
        }
    }
}

namespace AsteroiderCore.SaveSystem
{
    public interface ISaver
    {
        public void Save<T>(T saveObject, string path) where T : class;
        public T Load<T>(string path) where T : class, new();
        public bool FileExist(string path);
    }
}

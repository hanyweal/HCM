namespace HCMBLL
{
    public class GenericFactoryPattern<K, T> where T : class, K, new()
    {
        public static K CreateInstance()
        {
            K ObjK = new T();
            return ObjK;
        }
    }
}
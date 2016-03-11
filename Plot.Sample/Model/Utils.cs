namespace Plot.Sample.Model
{
    public class Utils
    {
        public static bool Equals<T>(T entity, object obj) where T : class
        {
            var other = obj as T;
            if (other == null)
            {
                return false;
            }
            return entity.GetHashCode() == other.GetHashCode();
        }
    }
}

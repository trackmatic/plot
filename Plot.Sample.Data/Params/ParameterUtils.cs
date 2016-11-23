namespace Plot.Sample.Data.Params
{
    public static class ParameterUtils
    {
        public static bool NotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}

namespace Plot.N4j
{
    public class Dataset<T1>
    {
        public T1 Item1 { get; set; }
    }

    public class Dataset<T1, T2>
    {
        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }
    }

    public class Dataset<T1, T2, T3>
    {
        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }

        public T3 Item3 { get; set; }
    }

    public class Dataset<T1, T2, T3, T4>
    {
        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }

        public T3 Item3 { get; set; }

        public T4 Item4 { get; set; }
    }

    public class Dataset<T1, T2, T3, T4, T5>
    {
        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }

        public T3 Item3 { get; set; }

        public T4 Item4 { get; set; }

        public T5 Item5 { get; set; }
    }
}

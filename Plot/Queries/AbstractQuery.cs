using System.IO;
using System.Xml.Serialization;

namespace Plot.Queries
{
    public abstract class AbstractQuery<TResult> : IQuery<TResult>
    {
        protected AbstractQuery()
        {
            Take = 128;
        }

        public int Take { get; set; }

        public int Skip { get; set; }

        public string[] OrderBy { get; set; }

        public bool Descending { get; set; }

        public IQuery<TResult> Next()
        {
            var clone = Clone(this);

            clone.Skip += Take;

            return clone;
        }

        public static T Clone<T>(T item)
        {
            var serializer = new XmlSerializer(item.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, item);
                stream.Position = 0;
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}

using System.IO;
using System.Xml.Serialization;

namespace Octo.Core.Artifacts
{
    public static class Cloner
    {
        public static T Clone<T>(T item)
        {
            var serializer = new XmlSerializer(item.GetType());

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, item);

                stream.Position = 0;

                return (T) serializer.Deserialize(stream);
            }
        }
    }
}
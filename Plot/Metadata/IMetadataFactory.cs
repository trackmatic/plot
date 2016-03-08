using System;

namespace Plot.Metadata
{
    public interface IMetadataFactory
    {
        NodeMetadata Create(Type type);

        NodeMetadata Create(object instance);
    }
}

namespace Plot.Proxies
{
    public interface ITrackableRelationship : ITrackable
    {
        object Current { get; }
    }
}

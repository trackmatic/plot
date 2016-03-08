namespace Plot
{
    public interface ITrackableRelationship : ITrackable
    {
        object Current { get; }
    }
}

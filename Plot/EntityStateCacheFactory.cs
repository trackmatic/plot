namespace Plot
{
    public class EntityStateCacheFactory : IEntityStateCacheFactory
    {
        public IEntityStateCache Create()
        {
            return new EntityStateCache();
        }
    }
}

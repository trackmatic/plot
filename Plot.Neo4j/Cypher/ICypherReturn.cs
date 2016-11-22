using Neo4j.Driver.V1;

namespace Plot.Neo4j.Cypher
{
    public interface ICypherReturn<out TResult>
    {
        ICypherReturn<TResult> Collect(Return item);
        ICypherReturn<TResult> CollectDistinct(Return item);
        ICypherReturn<TResult> Return(Return item);
        ICypherReturn<TResult> Collect(string property, string alias = null);
        ICypherReturn<TResult> CollectDistinct(string property, string alias = null);
        ICypherReturn<TResult> Return(string property, string alias = null);
        TResult Map(IRecord record);
    }
}

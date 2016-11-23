using System;
using Neo4j.Driver.V1;

namespace Plot.Neo4j.Cypher
{
    public interface ICypherQuery<T> : ICypherQuery
    {
        ICypherQuery<T> Skip(int count);
        ICypherQuery<T> Limit(int count);
        ICypherQuery<T> OrderByDescending(string property);
        ICypherQuery<T> OrderBy(string property);
        ICypherQuery<T> Return(Func<IRecord, T> map, Func<ICypherReturn<T>, ICypherReturn<T>> factory);
        ICypherQuery<T> ReturnDistinct(Func<IRecord, T> map, Func<ICypherReturn<T>, ICypherReturn<T>> factory);
        T Map(IRecord record);
    }
}

using System;
using Neo4j.Driver.V1;

namespace Plot.Neo4j.Cypher
{
    public interface ICypherFluentQuery<T> : ICypherFluentQuery
    {
        ICypherFluentQuery<T> Skip(int count);
        ICypherFluentQuery<T> Limit(int count);
        ICypherFluentQuery<T> OrderByDescending(string property);
        ICypherFluentQuery<T> OrderBy(string property);
        ICypherFluentQuery<T> Return(Func<IRecord, T> map, Func<ICypherReturn<T>, ICypherReturn<T>> factory);
        ICypherFluentQuery<T> ReturnDistinct(Func<IRecord, T> map, Func<ICypherReturn<T>, ICypherReturn<T>> factory);
        T Map(IRecord record);
    }
}

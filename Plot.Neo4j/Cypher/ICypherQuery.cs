using System.Collections.Generic;

namespace Plot.Neo4j.Cypher
{
    public interface ICypherQuery
    {
        string Statement { get; }
        IDictionary<string, object> Parameters { get; }
        string GetDebugText();
        ICypherQuery Match(string statement);
        ICypherQuery Set(string statement);
        ICypherQuery Merge(string statement);
        ICypherQuery With(string statement);
        ICypherQuery CreateUnique(string statement);
        ICypherQuery Delete(string statement);
        ICypherQuery Where(string statement);
        ICypherQuery OptionalMatch(string statement);
        ICypherQuery WithParam(string key, object value);
        ICypherQuery WithParam(string key, object[] value);
        ICypherQuery OnCreate();
        ICypherQuery OnMatch();
        bool ContainsParameter(string key);
        ICypherQuery<T> AsTypedQuery<T>();
    }
}

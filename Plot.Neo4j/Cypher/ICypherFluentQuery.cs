using System.Collections.Generic;

namespace Plot.Neo4j.Cypher
{
    public interface ICypherFluentQuery
    {
        string Statement { get; }
        IDictionary<string, object> Parameters { get; }
        string GetDebugText();
        ICypherFluentQuery Match(string statement);
        ICypherFluentQuery Set(string statement);
        ICypherFluentQuery Merge(string statement);
        ICypherFluentQuery With(string statement);
        ICypherFluentQuery CreateUnique(string statement);
        ICypherFluentQuery Delete(string statement);
        ICypherFluentQuery Where(string statement);
        ICypherFluentQuery OptionalMatch(string statement);
        ICypherFluentQuery WithParam(string key, object value);
        ICypherFluentQuery OnCreate();
        ICypherFluentQuery OnMatch();
        bool ContainsParameter(string key);
    }
}

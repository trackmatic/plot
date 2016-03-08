using Neo4jClient.Cypher;
using Octo.Core;
using Octo.N4j.Snippets;

namespace Octo.N4j.Commands
{
    public abstract class CreateReverseAssociationCommandBase<TSource, TDestination> : ICommand
    {
        private readonly ParamSnippet<TSource> _source;

        private readonly NodeSnippet<TDestination> _destination;

        private readonly string _relationship;

        protected CreateReverseAssociationCommandBase(ParamSnippet<TSource> source, NodeSnippet<TDestination> destination, string relationship)
        {
            _source = source;

            _destination = destination;

            _destination = destination;

            _relationship = relationship;
        }

        public ICypherFluentQuery Execute(ICypherFluentQuery query)
        {
            query = query
                .With(new WithSnippet(_source))
                .Match(new MatchNodeSnippet(_destination, new ParamSnippet(_destination.Param, "id")))
                .CreateUnique(new CreateUniqueSnippet(_source, _destination.Param, _relationship))
                .WithParam(new ParamSnippet(_destination.Param, "id"), EntityUtils.GetEntityId(_destination.Data));
            return query;
        }
    }
}

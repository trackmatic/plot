using Plot.Metadata;
using Plot.Neo4j;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Data.Results;

namespace Plot.Sample.Data.Mappers
{
    public class PersonMapper : Mapper<Person>
    {
        public PersonMapper(IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(session, transactionFactory, metadataFactory)
        {

        }
        
        protected override object GetData(Person item)
        {
            return new PersonNode(item);
        }

        protected override IQueryExecutor<Person> CreateQueryExecutor()
        {
            return CreateGenericExecutor(PersonResult.Map, PersonResult.Return);
        }
    }
}

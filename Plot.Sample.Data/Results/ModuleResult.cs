using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class ModuleResult : AbstractCypherQueryResult<Module>
    {
        public ModuleNode Module { get; set; }

        public override void Map(Module aggregate)
        {
            aggregate.Name = Module.Name;
        }

        public override Module Create()
        {
            var contact = Module.AsModule();
            return contact;
        }
    }
}

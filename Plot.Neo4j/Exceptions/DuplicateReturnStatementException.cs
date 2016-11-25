using System;
using Plot.Neo4j.Cypher;

namespace Plot.Neo4j.Exceptions
{
    public class DuplicateReturnStatementException : Exception
    {
        public DuplicateReturnStatementException(Return value)
            : base($"The return statement already contains proeprty name \"{value.Property}\"")
        {
            
        }
    }
}

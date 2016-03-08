namespace Octo.Core.Queries
{
    public interface IQuery
    {
        int Take { get; set; }

        int Skip { get; set; }

        string[] OrderBy { get; set; }

        bool Descending { get; set; }
    }
}

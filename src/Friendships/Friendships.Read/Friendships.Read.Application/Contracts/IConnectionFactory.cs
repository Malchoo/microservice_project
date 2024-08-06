using Microsoft.Data.SqlClient;

namespace Friendships.Read.Application.Contracts;

public interface IConnectionFactory
{
    public SqlConnection Create();
}

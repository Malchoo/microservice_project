using Friendships.Read.Application.Contracts;
using Microsoft.Data.SqlClient;

namespace Friendships.Read.Infrastructure.Dapper;

public class ConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public ConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection Create()
    {
        return new SqlConnection(_connectionString);
    }
}
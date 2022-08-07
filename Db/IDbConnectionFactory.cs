using System.Data;

namespace SG_TechTest.Db;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnection();
}
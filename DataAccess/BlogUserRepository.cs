using Dapper;
using MyBlog.Models;
using System.Data;
using System.Data.SqlClient;

namespace MyBlog.DataAccess
{
    public class BlogUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public BlogUserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetSection("pgSettings")["pgConnection"];

        }


        public bool IsAuthenticated(BlogUser blogUser)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {

                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT * ";
                sqlQuery += sqlQuery = "FROM blog_user ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND username = @Username ";
                sqlQuery += sqlQuery = "AND password = @Password ";


                var result = dbConnection.Query<BlogUser>(sqlQuery, blogUser).ToList();

                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public BlogUser GetStoredPasswordByUsername(string username)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT password ";
                sqlQuery += sqlQuery = "FROM blog_user ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND username = @Username ";

                var blogUser = dbConnection.QueryFirstOrDefault<BlogUser>(sqlQuery, new
                {
                    Username = username
                });

                return blogUser;
            }
        }

    }
}

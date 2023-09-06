using Dapper;
using MyBlog.Models;
using System.Data;
using System.Data.SqlClient;

namespace MyBlog.DataAccess
{
    public class BlogTagRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public BlogTagRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetSection("pgSettings")["pgConnection"];

        }

        public void CreateTag(int blogId, string tagName)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "INSERT INTO blog_tag ";
                sqlQuery += sqlQuery = "( ";
                sqlQuery += sqlQuery = "blog_id, ";
                sqlQuery += sqlQuery = "name, ";
                sqlQuery += sqlQuery = "created_at ";
                sqlQuery += sqlQuery = ") ";
                sqlQuery += sqlQuery = "VALUES ";
                sqlQuery += sqlQuery = "(";
                sqlQuery += sqlQuery = "@BlogTagBlogID, ";
                sqlQuery += sqlQuery = "@BlogTagName, ";
                sqlQuery += sqlQuery = "@BlogTagCreatedAt ";
                sqlQuery += sqlQuery = ")";

                dbConnection.Execute(sqlQuery, new
                {
                    BlogTagBlogID = blogId,
                    BlogTagName = tagName,
                    BlogTagCreatedAt = DateTime.UtcNow
                });
            }

        }

        public void DeleteBlogTag(int blogTagID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "DELETE ";
                sqlQuery += sqlQuery = "FROM blog_tag ";
                sqlQuery += sqlQuery = "WHERE blog_tag_id = @BlogTagID";

                dbConnection.Execute(sqlQuery, new
                {
                    BlogTagID = blogTagID
                });
            }
        }

        public IEnumerable<BlogTag> GetAllBlogTagsByBlogID(int blogID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT * ";
                sqlQuery += sqlQuery = "FROM blog_tag ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND blog_id = @BlogID ";

                var results = dbConnection.Query<BlogTag>(sqlQuery, new
                {
                    BlogID = blogID
                }).ToList();

                return results;

            }
        }

        public void DeleteAllBlogTagsByBlogID(int blogID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "DELETE ";
                sqlQuery += sqlQuery = "FROM blog_tag ";
                sqlQuery += sqlQuery = "WHERE blog_id = @BlogID";

                dbConnection.Execute(sqlQuery, new
                {
                    BlogID = blogID
                });
            }
        }
    }
}

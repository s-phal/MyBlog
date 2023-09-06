using Dapper;
using MyBlog.Models;
using System.Data;
using System.Data.SqlClient;

namespace MyBlog.DataAccess
{
    public class BlogCommentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        //private readonly NpgsqlConnection dbConnection;


        public BlogCommentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetSection("pgSettings")["pgConnection"];
        }

        public IEnumerable<dynamic> GetAllBlogCommentsForAdminCommentTable()
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT bc.blog_comment_id, blog.slug, blog.title, bc.display_name, bc.user_website, bc.content, bc.created_at, bc.active_flag ";
                sqlQuery += sqlQuery = "FROM blog_comment bc ";
                sqlQuery += sqlQuery = "LEFT JOIN blog ON blog.blog_id = bc.blog_id ";
                sqlQuery += sqlQuery = "WHERE 1 =1 ";
                sqlQuery += sqlQuery = "ORDER BY bc.created_at DESC ";

                return dbConnection.Query<dynamic>(sqlQuery).ToList();
            }
        }

        public IEnumerable<BlogComment> GetBlogCommentsByBlogID(int blogID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT * ";
                sqlQuery += sqlQuery = "FROM blog_comment ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND blog_id = @BlogID ";
                sqlQuery += sqlQuery = "AND active_flag = 1 ";
                sqlQuery += sqlQuery = "ORDER BY created_at DESC ";

                var blogComments = dbConnection.Query<BlogComment>(sqlQuery, new
                {
                    BlogID = blogID,
                }).ToList();

                return blogComments;
            }

        }

        public void CreateBlogComment(BlogComment blogComment)
        {

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "INSERT INTO blog_comment ";
                sqlQuery += sqlQuery = "( ";
                sqlQuery += sqlQuery = "blog_id, ";
                sqlQuery += sqlQuery = "content, ";
                sqlQuery += sqlQuery = "display_name, ";
                sqlQuery += sqlQuery = "user_website, ";
                sqlQuery += sqlQuery = "created_at, ";
                sqlQuery += sqlQuery = "active_flag ";
                sqlQuery += sqlQuery = ") ";
                sqlQuery += sqlQuery = "VALUES ";
                sqlQuery += sqlQuery = "( ";
                sqlQuery += sqlQuery = "@BlogID, ";
                sqlQuery += sqlQuery = "@CommentContent, ";
                sqlQuery += sqlQuery = "@DisplayName, ";
                sqlQuery += sqlQuery = "@UserWebsite, ";
                sqlQuery += sqlQuery = "@BlogCreatedAt, ";
                sqlQuery += sqlQuery = "@ActiveFlag ";
                sqlQuery += sqlQuery = ") ";

                dbConnection.Execute(sqlQuery, new
                {
                    BlogID = blogComment.blog_id,
                    CommentContent = blogComment.content,
                    DisplayName = blogComment.display_name,
                    UserWebsite = blogComment.user_website,
                    BlogCreatedAt = blogComment.created_at,
                    ActiveFlag = blogComment.active_flag
                });
            }


        }

        public void DeleteAllBlogCommentsByBlogID(int blogID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "DELETE ";
                sqlQuery += sqlQuery = "FROM blog_comment ";
                sqlQuery += sqlQuery = "WHERE blog_id = @BlogID";

                dbConnection.Execute(sqlQuery, new
                {
                    BlogID = blogID
                });
            }
        }

        public void ApproveBlogCommentByCommentID(int blogCommentID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "UPDATE blog_comment ";
                sqlQuery += sqlQuery = "SET active_flag = 1 ";
                sqlQuery += sqlQuery = "WHERE blog_comment_id = @blogCommentID";

                dbConnection.Execute(sqlQuery, new
                {
                    blogCommentID = blogCommentID
                });
            }
        }

        public void DeleteBlogCommentByCommentID(int blogCommentID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "DELETE FROM blog_comment ";
                sqlQuery += sqlQuery = "WHERE blog_comment_id = @blogCommentID";

                dbConnection.Execute(sqlQuery, new
                {
                    blogCommentID = blogCommentID
                });
            }

        }

    }
}

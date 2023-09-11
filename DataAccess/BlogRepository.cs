using Dapper;
using MyBlog.Core;
using MyBlog.Models;
using System.Data;
using System.Data.SqlClient;

namespace MyBlog.DataAccess
{
    public class BlogRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public BlogRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetSection("ConnectionStrings")["sqlServer"];

        }

        public Blog CreateBlog(Blog blog)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                blog.slug = CommonLib.ConvertTitleToSlug(blog.title);

                string sqlQuery = "";
                sqlQuery += sqlQuery = "INSERT INTO blog ";
                sqlQuery += sqlQuery = "( ";
                sqlQuery += sqlQuery = "title, ";
                sqlQuery += sqlQuery = "summary, ";
                sqlQuery += sqlQuery = "content, ";
                sqlQuery += sqlQuery = "slug, ";
                sqlQuery += sqlQuery = "created_at ";
                sqlQuery += sqlQuery = ") ";
                sqlQuery += sqlQuery = "VALUES ";
                sqlQuery += sqlQuery = "( ";
                sqlQuery += sqlQuery = "@title, ";
                sqlQuery += sqlQuery = "@summary, ";
                sqlQuery += sqlQuery = "@content, ";
                sqlQuery += sqlQuery = "@slug, ";
                sqlQuery += sqlQuery = "@created_at ";
                sqlQuery += sqlQuery = ") ";

                dbConnection.Execute(sqlQuery, blog);

                return GetBlogDetailsByTimeStamp(blog.created_at);
            }

        }

        public void UpdateBlog(Blog blog)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "UPDATE blog ";
                sqlQuery += sqlQuery = "SET title = @BlogTitle, ";
                sqlQuery += sqlQuery = "    summary = @BlogSummary, ";
                sqlQuery += sqlQuery = "    content = @BlogContent ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND blog_id = @BlogID";

                dbConnection.Execute(sqlQuery, new
                {
                    BlogTitle = blog.title,
                    BlogSummary = blog.summary,
                    BlogContent = blog.content,
                    BlogID = blog.blog_id
                });
            }
        }

        public void DeleteBlog(int blogID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "DELETE ";
                sqlQuery += sqlQuery = "FROM blog ";
                sqlQuery += sqlQuery = "WHERE blog_id = @BlogID ";

                dbConnection.Execute(sqlQuery, new
                {
                    BlogID = blogID
                });
            }
        }

        public Blog GetBlogDetailsBySlug(string slugName)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT * ";
                sqlQuery += sqlQuery = "FROM blog ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND slug = @SlugName ";


                var blog = dbConnection.QueryFirstOrDefault<Blog>(sqlQuery, new
                {
                    SlugName = slugName
                });

                return blog;
            }

        }

        public Blog GetBlogDetailsByID(int blogID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT * ";
                sqlQuery += sqlQuery = "FROM blog ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND blog_id = @BlogID ";

                var blog = dbConnection.QuerySingleOrDefault<Blog>(sqlQuery, new
                {
                    BlogID = blogID
                });

                return blog;
            }
        }

        public Blog GetBlogDetailsByTimeStamp(DateTime timeStamp)
        {

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT * ";
                sqlQuery += sqlQuery = "FROM blog ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND created_at = @TimeStamp ";

                var blog = dbConnection.QuerySingleOrDefault<Blog>(sqlQuery, new
                {
                    TimeStamp = timeStamp
                });

                return blog;
            }
        }

        public IEnumerable<dynamic> GetAllBlogsWithSummary()
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT blog.blog_id, blog.title, blog.slug, blog.summary, ISNULL(blog_comment.comment_count, 0) AS comment_count, STRING_AGG(blog_tag.name, ' ') AS tags, blog.created_at, bci.file_path ";
                sqlQuery += sqlQuery = "FROM blog ";
                sqlQuery += sqlQuery = "LEFT JOIN blog_tag ON blog.blog_id = blog_tag.blog_id ";
                sqlQuery += sqlQuery = "LEFT JOIN (SELECT blog_id, COUNT(*) AS comment_count FROM blog_comment WHERE blog_comment.active_flag = 1 GROUP BY blog_id) AS blog_comment ON blog.blog_id = blog_comment.blog_id ";
                sqlQuery += sqlQuery = "LEFT JOIN blog_cover_image bci on bci.blog_id = blog.blog_id ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "GROUP BY blog.blog_id, blog.title, blog.slug, blog.summary, blog_comment.comment_count, blog.created_at, bci.file_path ";
                sqlQuery += sqlQuery = "ORDER BY blog.created_at DESC ";

                var results = dbConnection.Query(sqlQuery).ToList();

                return results;
            }
        }

        public IEnumerable<dynamic> GetAllBlogsByTagName(string tagName)
        {

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT blog.blog_id, blog.title, blog.slug, blog.summary, ISNULL(blog_comment.comment_count, 0) AS comment_count, STRING_AGG(blog_tag.name, ' ') AS tags, blog.created_at, bci.file_path ";
                sqlQuery += sqlQuery = "FROM blog ";
                sqlQuery += sqlQuery = "LEFT JOIN blog_tag ON blog.blog_id = blog_tag.blog_id ";
                sqlQuery += sqlQuery = "LEFT JOIN (SELECT blog_id, COUNT(*) AS comment_count FROM blog_comment GROUP BY blog_id) AS blog_comment ON blog.blog_id = blog_comment.blog_id ";
                sqlQuery += sqlQuery = "LEFT JOIN blog_cover_image bci on bci.blog_id = blog.blog_id ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND blog_tag.name = @TagName ";
                sqlQuery += sqlQuery = "GROUP BY blog.blog_id, blog.title, blog.slug, blog.summary, blog_comment.comment_count, blog_tag.name, blog.created_at, bci.file_path ";
                sqlQuery += sqlQuery = "ORDER BY blog.created_at DESC ";

                var results = dbConnection.Query(sqlQuery, new
                {
                    TagName = tagName
                }).ToList();

                return results;
            }

        }

        public IEnumerable<dynamic> GetAllBlogsForAdminDataTable()
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT blog.blog_id, blog.title, blog.slug, COUNT(blog_comment.blog_id) AS comment_count, blog.created_at ";
                sqlQuery += sqlQuery = "FROM blog ";
                sqlQuery += sqlQuery = "LEFT JOIN blog_comment ON blog.blog_id = blog_comment.blog_id ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "GROUP BY blog.blog_id, blog.title, blog.slug, blog.created_at ";
                sqlQuery += sqlQuery = "ORDER BY created_at DESC ";

                dbConnection.Open();
                var results = dbConnection.Query(sqlQuery).ToList();
                dbConnection.Close();

                return results;
            }
        }


    }
}

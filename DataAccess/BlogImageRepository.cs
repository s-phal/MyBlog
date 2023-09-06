using Dapper;
using MyBlog.Models;
using System.Data;
using System.Data.SqlClient;

namespace MyBlog.DataAccess
{

    public class BlogImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public BlogImageRepository(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            connectionString = _configuration.GetSection("pgSettings")["pgConnection"];

        }

        public BlogCoverImage GetBlogCoverImageByBlogID(int blogID)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "SELECT * ";
                sqlQuery += sqlQuery = "FROM blog_cover_image ";
                sqlQuery += sqlQuery = "WHERE 1 = 1 ";
                sqlQuery += sqlQuery = "AND blog_id = @BlogID ";

                var blog = dbConnection.QueryFirstOrDefault<BlogCoverImage>(sqlQuery, new
                {
                    BlogID = blogID
                });

                return blog;
            }
        }

        public string UploadImage(IFormFile file)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public void CreateBlogCoverImage(int blogID, string filePath)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                sqlQuery += sqlQuery = "INSERT INTO blog_cover_image ";
                sqlQuery += sqlQuery = "( ";
                sqlQuery += sqlQuery = "blog_id, ";
                sqlQuery += sqlQuery = "file_path, ";
                sqlQuery += sqlQuery = "created_at, ";
                sqlQuery += sqlQuery = "active_flag ";
                sqlQuery += sqlQuery = ") ";
                sqlQuery += sqlQuery = "VALUES ";
                sqlQuery += sqlQuery = "( ";
                sqlQuery += sqlQuery = "@BlogID, ";
                sqlQuery += sqlQuery = "@FilePath, ";
                sqlQuery += sqlQuery = "@CreatedAt, ";
                sqlQuery += sqlQuery = "@ActiveFlag ";
                sqlQuery += sqlQuery = ") ";

                dbConnection.Execute(sqlQuery, new
                {
                    BlogID = blogID,
                    FilePath = filePath,
                    CreatedAt = DateTime.Now,
                    ActiveFlag = 1
                });
            }
        }

    }


}





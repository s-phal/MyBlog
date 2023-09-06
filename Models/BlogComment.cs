namespace MyBlog.Models
{
    public class BlogComment
    {
        public int blog_comment_id { get; set; }
        public int blog_id { get; set; }
        public string display_name { get; set; }
        public string user_website { get; set; }
        public string content { get; set; }
        public DateTime created_at { get; set; }
        public int active_flag { get; set; }

        public BlogComment()
        {
            created_at = DateTime.Now;
            active_flag = 0;
            display_name = "Anonymous";
        }
    }
}
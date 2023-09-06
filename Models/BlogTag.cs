namespace MyBlog.Models
{
    public class BlogTag
    {
        public int blog_tag_id { get; set; }
        public int blog_id { get; set; }
        public string name { get; set; }
        public DateTime created_at { get; set; }
        public int active_flag { get; set; }

        public BlogTag()
        {
            created_at = DateTime.Now;
            active_flag = 1;
        }
    }





}

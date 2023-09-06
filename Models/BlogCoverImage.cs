namespace MyBlog.Models
{
    public class BlogCoverImage
    {
        public int blog_cover_image_id { get; set; }
        public int blog_id { get; set; }
        public string file_path { get; set; }
        public DateTime created_at { get; set; }
        public int active_flag { get; set; }
    }
}

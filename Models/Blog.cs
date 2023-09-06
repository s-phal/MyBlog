namespace MyBlog.Models
{

    public class Blog
    {
        public int blog_id { get; set; }
        public string title { get; set; }
        public string slug { get; set; }
        public string summary { get; set; }
        public string content { get; set; }
        public DateTime created_at { get; set; }
        public int active_flag { get; set; }


        public Blog()
        {
            created_at = DateTime.Now;
            active_flag = 1;
            slug = "";
        }


    }
}

namespace MyBlog.Models
{
    public class BlogViewModel
    {
        // IEnumerable<> are more lightweight(Read Only)
        // compared to List<>(adding, removing, modifying )

        public Blog Blog { get; set; }

        public IEnumerable<BlogComment> BlogComments { get; set; }
        public IEnumerable<BlogTag> BlogTags { get; set; }
        public BlogCoverImage BlogCoverImage { get; set; }

    }
}

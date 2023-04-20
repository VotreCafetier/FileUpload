namespace FileUpload.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public string? ImageCaption { get; set; }
        public string? ImageDescription { set; get; }
        public IFormFile? MyImage { set; get; }
    }
}

namespace english_mastering_app
{
    public class SubtitleDto
    {
        public SubtitleDto(string title, string link) 
        {
            this.Title = title;
            this.Link = link;
        }
        
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
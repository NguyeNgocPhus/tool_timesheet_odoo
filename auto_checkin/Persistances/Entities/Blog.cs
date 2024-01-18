namespace auto_checkin.Persistances.Entities
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string ShortTitle { get;set; }
        public string Slug { get; set; }
        public string Content {  get; set; }
        public int View { get; set; }
        public Guid SerieId { get; set; }
        public int Order { get; set; }
        public virtual Series Series { get; set; }
        public virtual List<Comment> Comments { get; set; }

    }
}

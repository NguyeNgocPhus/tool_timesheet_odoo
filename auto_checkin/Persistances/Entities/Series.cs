namespace auto_checkin.Persistances.Entities
{
    public class Series : BaseEntity
    {
        public string Name { get; set; }    
        public string Slug { get; set; }
        public virtual List<Blog> Blogs { get; set; }
    }
}

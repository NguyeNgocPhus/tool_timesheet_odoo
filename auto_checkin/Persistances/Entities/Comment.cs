namespace auto_checkin.Persistances.Entities
{
    public class Comment : BaseEntity
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public Guid ParentId { get; set; }
        public Guid BlogId { get; set;}

        public virtual Blog Blog { get; set; }
    }
}

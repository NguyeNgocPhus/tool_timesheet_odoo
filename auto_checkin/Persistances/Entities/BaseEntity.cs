namespace auto_checkin.Persistances.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public int Status { get;set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;
    }
}

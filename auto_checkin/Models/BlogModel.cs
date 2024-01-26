using System.ComponentModel.DataAnnotations;

namespace auto_checkin.Models
{
    public class BlogModel
    {

        public string Title { get; set; }
        [UIHint("Html")]
        public string Full { get; set; }
        public int Comment { get;set; }
        public string CreateTime { get; set; }
        public string Series { get; set; }
        public List<string> BlogInSeries { get; set; }

    }
}

using System;

namespace RssFilter.Models
{
    public class Feed
    {
        public Guid Id { get; set; }
        public string LastItemId { get;set;}
        public string BaseUrl { get; set; }
        public string PublicUrl { get; set; }
        public string Name { get; set; }
        public DateTime? LastCheck { get; set; }
    }
}

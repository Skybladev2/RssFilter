using System;

namespace RssFilter.Models
{
    public class Keyword
    {
        public Guid Id { get; set; }
        public Feed Feed { get; set; }
        public string Text { get; set; }
    }
}

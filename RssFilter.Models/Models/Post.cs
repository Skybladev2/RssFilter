using System;

namespace RssFilter.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public Feed Feed { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        //public int IsPublishedToVk { get; set; }
    }
}
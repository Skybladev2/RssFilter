using Microsoft.AspNetCore.Mvc;
using RssFilter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace RssFilter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RssController : ControllerBase
    {
        private readonly SQLiteDb _context;

        public RssController(SQLiteDb context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult FavtDroneRss()
        {
            return GetCachedRss(_context.Feeds.FirstOrDefault());
        }

        private static IActionResult SerializeRss(SyndicationFeed rss)
        {
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true
            };
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, settings))
                {
                    var rssFormatter = new Rss20FeedFormatter(rss, false);
                    rssFormatter.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                stream.Position = 0;
                return new ContentResult { Content = new StreamReader(stream, Encoding.UTF8).ReadToEnd(), ContentType = "application/xml; charset=utf-8" };
            }
        }

        private IActionResult GetCachedRss(Feed feed)
        {
            var posts = _context.Posts.Where(x => x.Feed == feed).OrderByDescending(x => x.Link);
            var rss = new SyndicationFeed();
            var items = new LinkedList<SyndicationItem>();
            foreach (var post in posts)
            {
                items.AddLast(new LinkedListNode<SyndicationItem>(new SyndicationItem(post.Title, post.Content, new Uri(post.Link))));
            }
            rss.Items = items;
            return SerializeRss(rss);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RssFilter.Models;

namespace RssFilter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RssController : ControllerBase
    {
        private readonly PostgresDB _context;
        private readonly HttpClient client = new HttpClient();
        private readonly IConfiguration _configuration;

        public RssController(PostgresDB context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult FavtDroneRss()
        {
            var cacheDuration = GetCacheDuration();
            // we have single feed for now
            var feed = _context.Feeds.FirstOrDefault();
            if (feed.LastCheck != null && feed.LastCheck.Value + cacheDuration > DateTime.Now)
            {
                return GetCachedRss(feed);
            }
            return GetFilteredRss(feed);

            //return new ContentResult { Content = "lala", ContentType = "text/html" };
        }

        private IActionResult GetFilteredRss(Feed feed)
        {
            var url = feed.BaseUrl;
            var reader = XmlReader.Create(url);
            var originalRss = SyndicationFeed.Load(reader);
            var filteredRss = new SyndicationFeed(feed.Name, "Description", new Uri("http://localhost"));
            var filteredItems = new LinkedList<SyndicationItem>();

            foreach (var post in originalRss.Items.Take(10).TakeWhile(x => x.Id != feed.LastItemId))
            {
                var postUrl = post.Links.FirstOrDefault().Uri;
                var response = client.GetAsync(postUrl).Result;
                var pageContents = response.Content.ReadAsStringAsync().Result;
                var pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);
                var printVersionNode = pageDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'ico-print')]");
                printVersionNode.ParentNode.RemoveChild(printVersionNode);
                var headerNode = pageDocument.DocumentNode.SelectSingleNode("//h2");
                headerNode.ParentNode.RemoveChild(headerNode);
                var content = pageDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'news-info')]").InnerHtml;
                var rssContent = new TextSyndicationContent(content);
                filteredItems.AddLast(new LinkedListNode<SyndicationItem>(new SyndicationItem(post.Title.Text, content, postUrl)
                {
                    Summary = rssContent
                }));
                SavePost(feed, postUrl, post.Title.Text, content);
            }

            filteredRss.Items = filteredItems;
            feed.LastCheck = DateTime.Now;
            feed.LastItemId = originalRss.Items.First().Id;
            _context.Update(feed);
            _context.SaveChangesAsync();
            return SerializeRss(filteredRss);
        }

        private void SavePost(Feed feed, Uri postUrl, string title, string content)
        {
            var post = new Post() { Link = postUrl.ToString(), Title = title, Summary = content, Content = content, Feed = feed };
            if (_context.Entry(post).State == EntityState.Detached)
            {
                _context.Posts.Add(post);
            }
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
            var posts = _context.Posts.Where(x => x.Feed == feed);
            var rss = new SyndicationFeed(feed.Name, "Description", new Uri("http://localhost"));
            var items = new LinkedList<SyndicationItem>();
            foreach (var post in posts)
            {
                items.AddLast(new LinkedListNode<SyndicationItem>(new SyndicationItem(post.Title, post.Content, new Uri(post.Link))));
            }
            return SerializeRss(rss);
        }

        private TimeSpan GetCacheDuration()
        {
            var match = Regex.Match(_configuration["CacheDuration"], "(\\d+)m");
            return new TimeSpan(0, int.Parse(match.Groups[1].Value), 0);
        }
    }
}
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RssFilter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace DbUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new SQLiteDb();
            if (!Directory.Exists("db"))
            {
                Directory.CreateDirectory("db");
            }
            dbContext.Database.Migrate();
            var cacheDuration = GetCacheDuration();
            // we have single feed for now
            var feed = dbContext.Feeds.FirstOrDefault();
            if (feed.LastCheck != null && feed.LastCheck.Value + cacheDuration > DateTime.Now)
            {
                Console.WriteLine("Cached");
                return;
            }
            GetNewPosts(feed, dbContext);
        }

        private static void GetNewPosts(Feed feed, SQLiteDb context)
        {
            var url = feed.BaseUrl;
            var reader = XmlReader.Create(url);
            var originalRss = SyndicationFeed.Load(reader);
            var filteredRss = new SyndicationFeed();
            var filteredItems = new LinkedList<SyndicationItem>();
            var keywords = context.Keywords.Where(k => k.Feed.Id == feed.Id).Select(k => k.Text).ToHashSet();
            var itemsToProcessCount = 2000;

            if(feed.LastCheck != null)
            {
                itemsToProcessCount = 200;
            }

            Console.WriteLine("Original RSS item count: " + originalRss.Items.Count());
            Console.WriteLine("Processing count: " + itemsToProcessCount);

            foreach (var post in originalRss.Items.Take(200).TakeWhile(x => x.Id.CompareTo(feed.LastItemId) > 0))
            {
                try
                {
                    var postUrl = post.Links.FirstOrDefault().Uri;
                    var client = new HttpClient();
                    var response = client.GetAsync(postUrl).Result;
                    var pageContents = response.Content.ReadAsStringAsync().Result;
                    var pageDocument = new HtmlDocument();
                    pageDocument.LoadHtml(pageContents);
                    var printVersionNode = pageDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'ico-print')]");
                    if (printVersionNode == null || printVersionNode.ParentNode == null)
                    {
                        continue;
                    }
                    printVersionNode.ParentNode.RemoveChild(printVersionNode);
                    var headerNode = pageDocument.DocumentNode.SelectSingleNode("//h2");
                    headerNode.ParentNode.RemoveChild(headerNode);
                    var content = pageDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'news-info')]").InnerHtml;
                    var rssContent = new TextSyndicationContent(content);

                    if (keywords.Any(k => rssContent.Text.Contains(k)))
                    {
                        filteredItems.AddLast(new LinkedListNode<SyndicationItem>(new SyndicationItem(post.Title.Text, content, postUrl)
                        {
                            Summary = rssContent
                        }));
                        AddPost(context, feed, postUrl, post.Title.Text, content);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
                }
            }

            //filteredRss.Items = filteredItems;
            feed.LastCheck = DateTime.Now;
            feed.LastItemId = originalRss.Items.First().Id;
            context.Update(feed);
            context.SaveChangesAsync();
            //return SerializeRss(filteredRss);
        }

        private static TimeSpan GetCacheDuration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var match = Regex.Match(configuration["CacheDuration"], "(\\d+)m");
            return new TimeSpan(0, int.Parse(match.Groups[1].Value), 0);
        }

        private static void AddPost(SQLiteDb context, Feed feed, Uri postUrl, string title, string content)
        {
            if (context.Posts.Any(e => e.Link == postUrl.AbsoluteUri))
            {
                return;
            }

            var post = new Post() { Link = postUrl.ToString(), Title = title, Summary = content, Content = content, Feed = feed };

            if (context.Entry(post).State == EntityState.Detached)
            {
                context.Posts.Add(post);
            }
        }
    }
}

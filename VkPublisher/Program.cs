using Microsoft.EntityFrameworkCore;
using RssFilter.Models;
using System;
using System.IO;
using System.Linq;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started");
            var dbContext = new SQLiteDb();
            if (!Directory.Exists("db"))
            {
                Directory.CreateDirectory("db");
            }
            dbContext.Database.Migrate();

            var unpublishedPosts = dbContext.Posts;//.Where(p => !p.IsPublishedToVk);
            if (unpublishedPosts.Any())
            {
                var api = new VkApi();

                api.Authorize(new ApiAuthParams
                {
                    AccessToken = "b9e0a8e2f59c221a75148086d33e7f798867693336478fe62028a460c4c1cb95a4bdbd017e96c2704542e"
                });
                foreach (var post in unpublishedPosts)
                {
                    try
                    {
                        api.Wall.Post(new WallPostParams() { Message = post.Content, FromGroup = true, OwnerId = -202099852 });
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Cannot post news");
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            Console.WriteLine("Finished");
        }
    }
}

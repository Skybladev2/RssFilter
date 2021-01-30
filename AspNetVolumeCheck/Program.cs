using System;
using System.IO;

namespace AspNetVolumeCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var usersecrets = "/usersecrets";
            if (Directory.Exists(usersecrets))
            {
                var entries = Directory.EnumerateFileSystemEntries(usersecrets);
                foreach (var item in entries)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine(usersecrets + " does not exist");
            }
            var https = "/https";
            if (Directory.Exists(usersecrets))
            {
                var entries = Directory.EnumerateFileSystemEntries(https);
                foreach (var item in entries)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine(https + " does not exist");
            }
            Console.ReadKey();
        }
    }
}

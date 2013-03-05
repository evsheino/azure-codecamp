using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AzureCodeCamp.Models
{
    public class JoukkoVideo
    {
        public int ID { get; set; }
        public string path { get; set; }
    }

    public class JoukkoVideoDBContext : DbContext
    {
        public DbSet<JoukkoVideo> JoukkoVideos { get; set; }
    }
}
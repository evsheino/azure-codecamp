using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Web.Security;

namespace AzureCodeCamp.Models
{
    public class JoukkoVideo
    {
        public int ID { get; set; }
        [Required]
        public string path { get; set; }
        [Required]
        public DateTime timestamp { get; set; }
        [Required]
        public virtual UserProfile user { get; set; }
    }

    public class JoukkoVideoDBContext : DbContext
    {
        public DbSet<JoukkoVideo> JoukkoVideos { get; set; }
    }
}
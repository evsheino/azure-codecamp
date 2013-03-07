using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Web.Security;
using System.Web.Mvc;

namespace AzureCodeCamp.Models
{
    public class JoukkoVideo
    {
        public int ID { get; set; }
        [Required]
        [Display(Name="Title")]
        public string title { get; set; }
        [Required]
        [HiddenInput]
        public string path { get; set; }
        [Required]
        [HiddenInput]
        [Display(Name="Added")]
        public DateTime timestamp { get; set; }
        [Required]
        [HiddenInput]
        [Display(Name="User")]
        public virtual UserProfile user { get; set; }

        public JoukkoVideo()
        {
            timestamp = DateTime.Now;
        }
    }

    public class JoukkoVideoDBContext : DbContext
    {
        public DbSet<JoukkoVideo> JoukkoVideos { get; set; }
    }
}
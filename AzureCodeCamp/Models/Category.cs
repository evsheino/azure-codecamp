using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzureCodeCamp.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string name { get; set; }
        public virtual ICollection<JoukkoVideo> videos { get; set; }
    }
}
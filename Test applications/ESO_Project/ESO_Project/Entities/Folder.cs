using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ESO_Project.Models;

namespace ESO_Project.Entities
{
    public class Folder
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [MaxLength(50), MinLength(1)]
        public string Name { get; set; }
        public int Shared { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
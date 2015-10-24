using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESO_Project.Models;

namespace ESO_Project.Entities
{
    public class KeysVector
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string userId { get; set; }
        [HiddenInput]
        public DateTime SyncTime { get; set; }
        [Required]
        [HiddenInput]
        public byte[] key { get; set; }
        [Required]
        [HiddenInput]
        public byte[] vector { get; set; }

        [ForeignKey("userId")]
        public virtual ApplicationUser user { get; set; }
        
    }
}
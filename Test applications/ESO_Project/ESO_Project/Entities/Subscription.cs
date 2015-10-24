using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ESO_Project.Models;

namespace ESO_Project.Entities
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [HiddenInput]
        DateTime SubDateTime { get; set; }
        [HiddenInput]
        public int Type { get; set; }
        [Required]
        public int Active { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
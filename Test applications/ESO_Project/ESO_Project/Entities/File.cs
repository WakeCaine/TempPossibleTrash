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
    public class File
    {
        [Key]
        public int Id { get; set; }
        [HiddenInput]
        public DateTime SyncTime { get; set; }
        [Required]
        [MaxLength(50), MinLength(1)]
        public string Name { get; set; }
        [Required]
        public int IsRoot { get; set; }
        [Required]
        [HiddenInput]
        public string Type { get; set; }
        [Required]
        [HiddenInput]
        public int Size { get; set; }
        public int Shared { get; set; }
        public int FolderId { get; set; }


        [ForeignKey("FolderId")]
        public virtual Folder Folder { get; set; }
        
    }
}
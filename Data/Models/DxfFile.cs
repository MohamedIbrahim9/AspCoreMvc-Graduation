using Graduation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Data.Models
{
    public class DxfFile: File
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUser2")]
        public string FK_ApplicatioUserId { get; set; }
        public ApplicationUser ApplicationUser2 { get; set; }
        public DxfFile()
        {

        }
    }
}

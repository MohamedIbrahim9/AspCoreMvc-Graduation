using Graduation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Data.Models
{
    [Table("ObjFile")]
    public class ObjFile : File
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string FK_ApplicatioUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        public ObjFile()
        {

        }
    }
}

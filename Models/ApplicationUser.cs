using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Graduation.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Graduation.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("ApplicationUser")]
        public ICollection<ObjFile> ApplicationUserFiles { get; set; }

        [InverseProperty("ApplicationUser2")]
        public ICollection<DxfFile> ApplicationDxfFiles { get; set; }
        public ApplicationUser()
        {
            ApplicationUserFiles = new HashSet<ObjFile>();
            ApplicationDxfFiles = new HashSet<DxfFile>();
        }
    }
}

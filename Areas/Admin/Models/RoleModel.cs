using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Role name is required"), Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
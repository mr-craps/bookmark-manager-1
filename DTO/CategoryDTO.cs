using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class CategoryDTO
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "The Name field is mandatory.")]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
    }
}

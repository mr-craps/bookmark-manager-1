using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class BookmarkDTO
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "The URL field is mandatory.")]
        [StringLength(maximumLength: 500)]
        //[Url]
        public string URL { get; set; }

        [Required(ErrorMessage = "The Short Description field is mandatory.")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "The Category field is mandatory.")]
        public int CategoryId { get; set; }

        //[Required(ErrorMessage = "The Category Name field is mandatory.")]
        //[StringLength(maximumLength: 50)]
        public string CategoryName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BookStore.Models
{
    public class BookModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the title of your book")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter the author name")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Please enter the description")]
        public string Description { get; set; }
        public string Category { get; set; }
        [Required(ErrorMessage= "Choose your book language")]
        [Display(Name = "Book Language")]
        public int LanguageId { get; set; }
        public string Language { get; set; }
        [Required(ErrorMessage = "Please enter the total pages")]
        [Display(Name = "Total pages of book")]
        public int? TotalPage { get; set; }
        [Required(ErrorMessage = "Please choose cover photo")]
        [Display(Name = "Cover Photo")]
        public IFormFile CoverPhoto { get; set; }
        public string CoverImageUrl { get; set; }
        
        [Required(ErrorMessage = "Please choose book photos")]
        [Display(Name = "Cover Photo")]
        public IFormFileCollection GalleryFiles { get; set; }
        public List<GalleryModel> Gallery { get; set; }
        
        [Required(ErrorMessage = "Please choose book in pdf format")]
        [Display(Name = "Book PDF")]
        public IFormFile BookPdf { get; set; }
        public string BookPdfUrl { get; set; }
    }
}
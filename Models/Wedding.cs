using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace weddingplanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingID { get; set; }

        [Required]
        [Display(Name = "Wedder One")]
        public string WedderOne { get; set; }

        [Required]
        [Display(Name = "Wedder Two")]
        public string WedderTwo { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        public int UserID { get; set; }

        public List<Guest> Attendees { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
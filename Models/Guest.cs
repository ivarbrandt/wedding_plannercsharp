using System;
using System.ComponentModel.DataAnnotations;

namespace weddingplanner.Models
{
    public class Guest
    {
        [Key]
        public int GuestID { get; set; }
        public int UserID { get; set; }
        public int WeddingID { get; set; }

        public User User { get; set; }
        public Wedding Wedding { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
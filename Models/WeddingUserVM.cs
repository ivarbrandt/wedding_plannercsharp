using System;
using System.Collections.Generic;

namespace weddingplanner.Models
{
    public class WeddingUserVM
    {
        public List<Wedding> AllWeddings { get; set; }
        public User User { get; set; }
        public Guest Guest { get; set; }
    }
}
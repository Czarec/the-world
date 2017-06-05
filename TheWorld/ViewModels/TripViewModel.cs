using System;
using System.ComponentModel.DataAnnotations;

namespace TheWorld.ViewModels
{
    public class TripViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}

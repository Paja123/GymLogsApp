using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TrainingSession
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(36)]
        public string UserId { get; set; } 
        [Required]
        public TrainingType TrainingType { get; set; }
        [Required]
        public int Duration { get; set; } // in minutes
        public int? CaloriesBurned { get; set; }
        [Required]
        public int IntensityLevel { get; set; } // Scale of 1-10
        [Required]
        public int TirednessLevel { get; set; } // Scale of 1-10
        public DateTime Date { get; set; }
        [MaxLength(300)]
        public string? Notes { get; set; }
    }
}

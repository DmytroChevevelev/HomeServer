using System.ComponentModel.DataAnnotations;

namespace HomeWebApi.Data
{
    public class Sensor
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string Description { get; set; }
        
        [Required]
        public string Type { get; set; }
        
        [Required]
        public string Location { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime LastUpdated { get; set; }
    }
}
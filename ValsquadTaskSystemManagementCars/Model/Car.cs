using System.ComponentModel.DataAnnotations;

namespace ValsquadTaskSystemManagementCars.Model
{
    public class Car
    {
        [Key]
        [MaxLength(10)]
        public string PlateNumber { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
    }
}

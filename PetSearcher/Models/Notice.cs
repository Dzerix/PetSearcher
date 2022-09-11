using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSearcher.Models
{
    public class Notice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Вид питомца")]
        public string KindOfPet { get; set; } = string.Empty;

        [Display(Name="Кличка питомца")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Display(Name ="Описание")]
        [MaxLength(300,ErrorMessage ="Максимальная длина описания 300 символов")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Место")]
        [Required]
        public string Location { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        public string ImageName { get; set; } = string.Empty;

        [Required]
        [NotMapped]
        [Display(Name ="Фото питомца")]
        public IFormFile ImageFile { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}

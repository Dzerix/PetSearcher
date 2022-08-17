//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PetSearcher.Models.ViewModels
//{
//    public class NoticeViewModel
//    {
//        [Key]
//        public int? Id { get; set; }

//        [Required]
//        [Display(Name = "Вид питомца")]
//        public string KindOfPet { get; set; } = string.Empty;

//        [Required]
//        public string Name { get; set; } = string.Empty;

//        public string Description { get; set; } = string.Empty;

//        public string ImagePath { get; set; }

//        [Required]   
//        [NotMapped]
//        [DisplayName]
//        public IFormFile Image { get; set; }

//        public string UserId { get; set; } = string.Empty;
//    }
//}

﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PetSearcher.Models.ViewModels
{
    public class EditNoticeViewModel
    {
        [Display(Name = "Кличка питомца")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        [MaxLength(300, ErrorMessage = "Максимальная длина описания 300 символов")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Место")]
        [Required]
        public string Location { get; set; } = string.Empty;
    }
}

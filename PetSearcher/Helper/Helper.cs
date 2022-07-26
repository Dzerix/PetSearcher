﻿using Microsoft.AspNetCore.Mvc.Rendering;
using PetSearcher.Models;

namespace PetSearcher.Helper
{
    public static class HelperClass
    {
        public static string Support = "Support";
        public static string User = "User";

        public static List<SelectListItem> GetRolesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Value = HelperClass.Support, Text = HelperClass.Support},
                new SelectListItem{Value = HelperClass.User, Text = HelperClass.User}
            };
        }

        public static List<SelectListItem> GetPetTypesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Text= "Кот",Value = "Кот"},
                new SelectListItem{Text= "Собака", Value = "Собака"}
            };
        }

        
    }
}

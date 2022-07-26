using Microsoft.AspNetCore.Mvc.Rendering;

namespace PetSearcher.Helper
{
    public static class Helper
    {
        public static string Support = "Support";
        public static string User = "User";

        public static List<SelectListItem> GetRolesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Value = Helper.Support, Text = Helper.Support},
                new SelectListItem{Value = Helper.User, Text = Helper.User}
            };
        }
    }
}

using PetSearcher.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PetSearcher.Services
{
    public class NoticeService : INoticeService
    {
        private readonly ApplicationDbContext _context;

        public NoticeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, string> GetUsersPhonesList()
        {

            Dictionary<string, string> list = new Dictionary<string, string>();
            foreach (var user in _context.Users)
            {
                list.Add(user.Id, user.PhoneNumber);
            }
            return list;
        }
    }
}

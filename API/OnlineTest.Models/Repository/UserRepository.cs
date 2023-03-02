using OnlineTest.Data;
using OnlineTest.Model.Interfaces;
using OnlineTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTest.Model.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineTestContext _context;
        public UserRepository(OnlineTestContext context)
        {
            _context = context;
        }
        public bool AddUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }
        public bool UpdateUser( User user)
        {
            _context.Update(user);
            return _context.SaveChanges() > 0;
        }
        public bool DeleteUser(int id)
        {
            var entity = _context.Users.FirstOrDefault(u => u.Id == id);
            if (entity != null)
            {
                _context.Remove(entity);
            }
            return _context.SaveChanges() > 0;
        }

    }
}

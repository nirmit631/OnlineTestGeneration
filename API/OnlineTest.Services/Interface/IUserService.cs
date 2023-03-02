using OnlineTest.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTest.Services.Interface
{
    public interface IUserService
    {
        List<UserDTO> GetUsers();
        bool AddUser(UserDTO user);
        bool UpdateUser(UserDTO user);
        bool DeleteUser(int id);
    }
}

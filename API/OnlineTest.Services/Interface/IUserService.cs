using OnlineTest.Models;
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
        List<UserDTO> GetUserPagination(int PageNo, int RowsPerPage);
        UserDTO GetUserbyId(int id);
        UserDTO GetUserbyEmail(string email);
        bool AddUser(UserDTO user);
        bool UpdateUser(UserDTO user);
        bool DeleteUser(int id);
        UserDTO IsUserExists(TokenDTO model);
    }
}

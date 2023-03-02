using Azure.Core;
using OnlineTest.Model.Interfaces;
using OnlineTest.Models;
using OnlineTest.Services.DTO;
using OnlineTest.Services.Interface;


namespace OnlineTest.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public bool AddUser(UserDTO user)
        {
            return _userRepository.AddUser(new User
            {
                Name = user.Name,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Password = user.Password,
                IsActive = user.IsActive
            });
        }

        public List<UserDTO> GetUsers()
        {
            try
            {
                var users = _userRepository.GetUsers().Select(s => new UserDTO()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    MobileNumber = s.MobileNumber,
                    Password = s.Password,
                    IsActive = s.IsActive

                }).ToList();
                return users;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool UpdateUser( UserDTO user)
        {
            return _userRepository.UpdateUser(new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Password = user.Password,
                IsActive = user.IsActive
            });
        }
        public bool DeleteUser(int id)
        {
            return _userRepository.DeleteUser(id);
        }
      
    }
}

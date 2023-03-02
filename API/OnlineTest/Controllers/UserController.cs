using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTest.Services.DTO;
using OnlineTest.Services.Interface;

namespace OnlineTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<UserDTO> Get()
        {
            var data = _userService.GetUsers();
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post(UserDTO user)
        {
            return Ok(_userService.AddUser(user));
        }
        [HttpPut]
        public IActionResult Put(UserDTO user)
        {
            return Ok(_userService.UpdateUser(user));

        }

        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            return Ok(_userService.DeleteUser(id));
        }

    }
}
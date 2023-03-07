using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTest.Services.DTO;
using OnlineTest.Services.Interface;

namespace OnlineTest.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get")]
        public ActionResult<UserDTO> Get(int? PageNo, int? RowsPerPage)
        {
            //var data = _userservice.getusers();
            //return Ok(data);
            List<UserDTO> data;
            if (PageNo.HasValue && RowsPerPage.HasValue)
                data = _userService.GetUserPagination(PageNo.Value, RowsPerPage.Value);
            else
                data = _userService.GetUsers();
            return Ok(data);

        }

        [HttpPost("add")]
        public IActionResult Post(UserDTO user)
        {
            //return Ok(_userService.AddUser(user));
            var result = _userService.GetUserbyEmail(user.Email);
            if (result == null)
            {
                return Ok(_userService.AddUser(user));
            }
            else
            {
                return BadRequest("User Already Exist");
            }
        }
        [HttpPut("update")]
        public IActionResult Put(UserDTO user)
        {
            var result = _userService.GetUserbyId(user.Id);
            if (result == null)
            {
                return NotFound("User Not found");
            }
            var resultEmail = _userService.GetUserbyEmail(user.Email);
            if (resultEmail != null && resultEmail.Id != user.Id)
            {
                return BadRequest("This email is already exist");

            }
            return Ok(_userService.UpdateUser(user));

        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {

            var result = _userService.GetUserbyId(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(_userService.DeleteUser(id));
        }

    }
}
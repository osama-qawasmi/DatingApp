using API.Data;
using API.Enitities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/users
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }
        [HttpGet("{id}")]
        public ActionResult<AppUser> GetUser(int Id)
        {
            var user = _context.Users.Find(Id);
            return user;
        }
    }
}
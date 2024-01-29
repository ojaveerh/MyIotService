using Database.Data.Models;
using Database.Data.Models.Auth;
using DatabaseService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ServiceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAccountsController : ControllerBase
    {
        private readonly IUserAccountsService _usersService;
        public UserAccountsController(IUserAccountsService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAccounts()
        {
            return Ok(await _usersService.GetUserAccountsAsync());
        }

        [HttpGet]
        [Route("{userId}/GetUserAccount")]
        public async Task<IActionResult> GetUserAccount(int userId)
        {
            return Ok(await _usersService.GetUserAccountAsync(userId));
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/GetUserAccountsByNameAsync")]
        public async Task<IActionResult> GetUserAccountsByNameAsync([FromBody] UserLogin userAccount)
        {
            return Ok(await _usersService.GetUserAccountsByNameAsync(userAccount));
        }

        [HttpPost]
        [Route("/CreateUserAccount")]
        public async Task<IActionResult> CreateUserAccountAsync(string username, string password)
        {
            var result = await _usersService.CreateUserAccountAsync(username, password);

            if (result == false)
            {
                return BadRequest($"User creation was unsuccessful! Faulty username. ");
            }

            return Ok("Account created!");
        }
    }
}

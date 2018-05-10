using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using dotnetFun.API.Data;
using dotnetFun.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetFun.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IDotnetAppRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDotnetAppRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            
            var usersDataFormated = _mapper.Map<IEnumerable<UserForDetailsDto>>(users);

            return Ok(usersDataFormated);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userDataFormated = _mapper.Map<UserForDetailsDto>(user);

            return Ok(userDataFormated);

        }
    }
}
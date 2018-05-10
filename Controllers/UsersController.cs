using System;
using System.Collections.Generic;
using System.Security.Claims;
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

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _repo.GetUser(username);

            var userDataFormated = _mapper.Map<UserForDetailsDto>(user);

            return Ok(userDataFormated);

        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] UserForUpdateDto userForUpdateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var loggedUserName = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(username);

            if (userFromRepo == null)
                return NotFound($"L'utilisateur {username} n'existe pas");

            if (loggedUserName != userFromRepo.Id)
                return Unauthorized();

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"[PUT] Opération impossible.");
        }
    }
}
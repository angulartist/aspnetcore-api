using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotnetFun.API.Data;
using dotnetFun.API.Dtos;
using dotnetFun.API.Helpers;
using dotnetFun.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace dotnetFun.API.Controllers
{
    [Authorize]
    [Route("api/users/{username}/photos")]
    public class PhotosController : Controller
    {
        private readonly IDotnetAppRepository _repo;

        private readonly IMapper _mapper;

        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        private readonly Cloudinary _cloudinary;

        public PhotosController(IDotnetAppRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("id", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id){
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForCreationDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserPhoto(string username, PhotoForCreationDto photoDto)
        {
            var user = await _repo.GetUser(username);

            if (user == null)
                return BadRequest("Utilisateur introuvable");

            var loggedUserName = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (loggedUserName != user.Id)
                return Unauthorized();

            var file = photoDto.File;

            var uploadRes = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadPrams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadRes = _cloudinary.Upload(uploadPrams);
                }
            }

            photoDto.Url = uploadRes.Uri.ToString();

            photoDto.publicId = uploadRes.PublicId;

            var photo = _mapper.Map<Photo>(photoDto);

            photo.User = user;

            if (!user.Photos.Any(m => m.isMain))
                photo.isMain = true;

            user.Photos.Add(photo);

            var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);

            return BadRequest("Nn..");
        }
    }
}
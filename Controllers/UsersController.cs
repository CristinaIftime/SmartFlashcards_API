using AutoMapper;
using LibrarieModele;
using NivelAccessDate;
using SmartFlashcards_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace SmartFlashcards_API.Controllers
{
    public class UsersController : ApiController
    {
        private readonly UsersAccessor _users = new UsersAccessor();
        private IMapper Mapper => WebApiApplication.MapperInstance;

        [HttpGet]
        public IEnumerable<UserDto> Get()
        {
            return _users.GetAll().Select(u => Mapper.Map<UserDto>(u));
        }

        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            var u = _users.GetById(id);
            if (u == null) return NotFound();
            return Ok(Mapper.Map<UserDto>(u));
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] CreateUserDto dto)
        {
            if (dto == null) return BadRequest("Body-ul request-ului este gol.");
            if (string.IsNullOrWhiteSpace(dto.email)) return BadRequest("Email lipsă.");
            if (string.IsNullOrWhiteSpace(dto.password)) return BadRequest("Parola lipsă.");

            if (_users.ExistsByEmail(dto.email))
                return BadRequest("Email deja existent.");

            var user = new User
            {
                user_id = Guid.NewGuid(),
                email = dto.email,
                password_hash = Hash(dto.password)
            };

            _users.Add(user);

            return Created(
                new Uri(Request.RequestUri + "/" + user.user_id),
                Mapper.Map<UserDto>(user)
            );
        }

        private static byte[] Hash(string password)
        {
            using (var sha = SHA256.Create())
                return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}

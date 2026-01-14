using NivelAccessDate;
using SmartFlashcards_API.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace SmartFlashcards_API.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly UsersAccessor _users = new UsersAccessor();

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginDto dto)
        {
            if (dto == null) return BadRequest("Body-ul request-ului este gol.");
            if (string.IsNullOrWhiteSpace(dto.email)) return BadRequest("Email lipsă.");
            if (string.IsNullOrWhiteSpace(dto.password)) return BadRequest("Parola lipsă.");

            var user = _users.GetByEmail(dto.email);
            if (user == null) return Unauthorized();

            var hash = Hash(dto.password);
            if (!SameBytes(user.password_hash, hash)) return Unauthorized();

            return Ok(new LoginResultDto { user_id = user.user_id, email = user.email });
        }

        private static byte[] Hash(string password)
        {
            using (var sha = SHA256.Create())
                return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool SameBytes(byte[] a, byte[] b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }
    }
}

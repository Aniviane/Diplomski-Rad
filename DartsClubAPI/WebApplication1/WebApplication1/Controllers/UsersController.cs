using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApplication1.Models.DTO_s;
using WebApplication1.Models.Framework.Models;
using WebApplication1.Models.Framework_Models;
using Nest;
using WebApplication1.Models.Framework.Models.DTOs;
using BCrypt.Net;



namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly IDistributedCache _redisCache;


        private string serverPath = "https://localhost:7133";
        public UsersController(DataContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _redisCache = distributedCache;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.Include(x => x.Reservations).Include(x => x.Picture).ToListAsync();
            var returningUsers = new List<UserDTO>();
            foreach (var user in users)
            {
                returningUsers.Add(new UserDTO(user));
            }
            return returningUsers;
        }

        [HttpGet("Usernames")]
        public async Task<ActionResult<IEnumerable<UsernameDTO>>> GetUsernames()
        {
            var users = await _context.Users.ToListAsync();
            var returningUsers = new List<UsernameDTO>();
            foreach (var user in users)
            {
                returningUsers.Add(new UsernameDTO(user));
            }
            return returningUsers;
        }


        [HttpPost("Picture")]
        public async Task<string> SetPicture(UploadPictureDTO dto)
        {

            var user = _context.Users.Find(dto.UserId);

            if (user == null) return "invalid user";

            if (user != null && dto.Picture.Length > 0)
            {

                var folderName = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Pictures", "Users", folderName);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var imageExtension = Path.GetExtension(dto.Picture.FileName);

                var imageName = "image" + imageExtension;

                var imagePath = Path.Combine(folderPath, imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    dto.Picture.CopyTo(stream);
                }

                var pic = new Picture();
                pic.imagePath = serverPath + "/Pictures/Users/" + folderName + "/" + imageName;
                pic.UserId = user.ID;

                _context.Pictures.Add(pic);
                user.Picture = pic;
                _context.SaveChanges();

                return pic.imagePath;

            }

                return "picture set?";
        }


        [HttpPut("Picture")]
        public async Task<string> UpdatePicture(UploadPictureDTO dto)
        {
            var user = _context.Users.Find(dto.UserId);

            if (user == null) return "invalid user";

            if (user.Picture == null)
            {
                if (dto.Picture.Length > 0)
                {

                    var folderName = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Pictures", "Users", folderName);

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var imageExtension = Path.GetExtension(dto.Picture.FileName);

                    var imageName = "image" + imageExtension;

                    var imagePath = Path.Combine(folderPath, imageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        dto.Picture.CopyTo(stream);
                    }

                    var pic = new Picture();
                    pic.imagePath = serverPath + "/Pictures/Users/" + folderName + "/" + imageName;
                    pic.UserId = user.ID;

                    _context.Pictures.Add(pic);
                    user.Picture = pic;
                    _context.SaveChanges();

                    return pic.imagePath;
                }

                return "something wrong with the picture";
            }
            
            
                var relativePath = user.Picture.imagePath.Replace(serverPath, "");

                var absolutePath = Directory.GetCurrentDirectory() + relativePath;

                absolutePath = absolutePath.Replace("/", "\\");

                if (dto.Picture != null && dto.Picture.Length > 0)
                {

                    if (System.IO.File.Exists(absolutePath))
                        System.IO.File.Delete(absolutePath);

                    var folderName = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Pictures", "Users", folderName);

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var imageExtension = Path.GetExtension(dto.Picture.FileName);

                    var imageName = "image" + imageExtension;

                    var imagePath = Path.Combine(folderPath, imageName);



                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {

                        dto.Picture.CopyTo(stream);
                    }

                    var pic = new Picture();
                    pic.imagePath = serverPath + "/Pictures/Users/" + folderName + "/" + imageName;
                    pic.UserId = user.ID;

                    user.Picture = pic;

                    _context.SaveChanges();

                    return pic.imagePath;
                }
            

                return "Something wrong with the picture";
            
        }

        [HttpPost("Login")]
        public  ActionResult<User> LoginUser([FromBody] LoginDTO loginDTO)
        {


            var user =  _context.Users.Where(x =>  x.Name == loginDTO.Username).FirstOrDefault();

            if (user == null || user.ID == null) return BadRequest();


            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password)) return BadRequest();


            List<Claim> claims =
                [
                    new Claim(ClaimTypes.Role, user.isModerator.ToString()),
                    new Claim(ClaimTypes.SerialNumber, user.ID.ToString()),
                ];
                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TrueMovieAwardsTrueMovieAwardsTrueMovieAwardsTrueMovieAwards"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:7133/",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signinCredentials
                    ); ;
                string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);





                _redisCache.SetString(user.ID.ToString(), token);

                

                var item = _redisCache.GetString(user.ID.ToString());

                return user;
            
        }



        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid? id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid? id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserCreateDTO user)
        {

            var User = new User(user);

            var passHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            User.Password = passHash;


            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = User.ID }, User);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid? id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid? id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}

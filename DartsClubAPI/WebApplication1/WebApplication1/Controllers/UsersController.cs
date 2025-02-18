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


        private string serverPath = "https://localhost:44314";
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
        public async Task<Picture?> SetPicture(UploadPictureDTO dto)
        {

            var user = _context.Users.Find(dto.UserId);

            if (user == null) return null;

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

                return pic;

            }

            return null;
        }


        [HttpPut("Picture")]
        public async Task<Picture?> UpdatePicture(UploadPictureDTO dto)
        {
            var user = _context.Users.Where(user => user.ID == dto.UserId).Include(user => user.Picture).FirstOrDefault();

            if (user == null) return null;

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

                    return pic;
                }

                return null;
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

                  
                    user.Picture.imagePath = serverPath + "/Pictures/Users/" + folderName + "/" + imageName;
                   

                    _context.SaveChanges();

                    return user.Picture;
                }


            return null;
            
        }

        [HttpPut("Password")]
        public async Task<bool> ChangePassword(UpdateUserDTO dto)
        {
            var user = _context.Users.Find(dto.Id);

            if (user == null) return false;


            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password)) return false;

            var passHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            user.Password = passHash;
            try {


               await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        [HttpPost("Login")]
        public  ActionResult<UserDTO> LoginUser([FromBody] LoginDTO loginDTO)
        {


            var user =  _context.Users.Where(x =>  x.Name == loginDTO.Username).Include(user => user.Reservations).Include(user => user.Picture).FirstOrDefault();

            if (user == null || user.ID == null) return BadRequest();


            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password)) return BadRequest();


            List<Claim> claims =
                [
                    new Claim(ClaimTypes.Role, user.isModerator.ToString()),
                    new Claim(ClaimTypes.SerialNumber, user.ID.ToString()),
                    new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
                ];
                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TrueMovieAwardsTrueMovieAwardsTrueMovieAwardsTrueMovieAwards"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44314/",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signinCredentials
                    ); ;
                string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);





                _redisCache.SetString(user.ID.ToString(), token);

                


                return new UserDTO(user);
            
        }



        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid? id)
        {
            var user = _context.Users.Where(user => user.ID == id).Include(user => user.Picture).Include(user => user.Reservations).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            List<Claim> claims =
               [
                   new Claim(ClaimTypes.Role, user.isModerator.ToString()),
                    new Claim(ClaimTypes.SerialNumber, user.ID.ToString()),
                    new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
                ];
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TrueMovieAwardsTrueMovieAwardsTrueMovieAwardsTrueMovieAwards"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:44314/",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
                ); ;
            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);




            _redisCache.SetString(user.ID.ToString(), token);


            return new UserDTO(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutUser(Guid? id, User data)
        {
            var user = _context.Users.Where(user => user.ID == id).FirstOrDefault();



            return "";
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(UserCreateDTO user)
        {

            var User = new User(user);

            User.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            
            _context.Users.Add(User);

            await _context.SaveChangesAsync();

            return Ok(User);
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

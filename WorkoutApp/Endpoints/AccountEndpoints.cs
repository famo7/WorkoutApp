using GymApp.DTO;
using GymApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GymApp.Endpoints
{
    public static class AccountEndpoints
    {

        public static void MapAccountEndpoints(this WebApplication app)
        {

            app.MapPost("/register", async (RegisterDTO registerDTO, GymContext db) =>
            {
                var user = await db.Users.AnyAsync(user => user.UserName == registerDTO.UserName);

                var email = await db.Users.AnyAsync(user => user.Email == registerDTO.Email);

                if (user) return Results.BadRequest("UserName is taken");
                if (email) return Results.BadRequest("Email is taken");

                var PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

                var newUser = new AppUser()
                {
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email,
                    PasswordHash = PasswordHash
                };

                db.Users.Add(newUser);
                await db.SaveChangesAsync();
                return Results.Ok(newUser);
            }).AllowAnonymous();

            app.MapPost("/login", async (LoginDTO loginDto, GymContext db) =>
            {
                var user = db.Users.SingleOrDefault(b => b.UserName == loginDto.UserName);


                if (user == null) return Results.BadRequest("User Not found!");
                var PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return Results.BadRequest("Wrong Credentials");
                }

                string jwt = CreateToken(app, user);
                return Results.Ok(jwt);
            }).AllowAnonymous();

        }

        private static string CreateToken(WebApplication app, AppUser? user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName!)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
             app.Configuration.GetSection("jwt:Token").Value!
            ));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
             claims: claims,
             expires: DateTime.Now.AddDays(7),
             signingCredentials: cred

            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
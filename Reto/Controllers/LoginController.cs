using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Reto.DBContext;
using Reto.Models;

namespace Reto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public LoginController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        // /api/Login/Autorizar
        [HttpPost("Autorizar")]
        public async Task<IActionResult> PostLogin([FromBody] Login log)
        {
            try
            {
                var consulta = await _appDbContext.Logins.Where(x => x.Email == log.Email && x.Password == log.Password).FirstAsync();
                if (consulta != null)
                {
                    var email = log.Email;
                    var typ = log.TypeUser;
                    var token = generartoken(email, typ);
                    return Ok(token);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            string generartoken(string email, string typ)
            {
                //header-firma
                var header = new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("TAQWERTYUIOP092ALADDIN")),
                        SecurityAlgorithms.HmacSha256));
                //Payload
                var claimss = new Claim[]
                {
                new Claim(JwtRegisteredClaimNames.UniqueName, email),
                new Claim(ClaimTypes.Role,typ),
                new Claim("Valor", "SalonDeClase")
                };
                var payloadd = new JwtPayload(
                    claims: claimss,
                    notBefore: DateTime.Now,
                    expires: DateTime.UtcNow.AddHours(1),
                    issuer: "https://localhost:7063/",
                    audience: "https://localhost:7063/");
                var token = new JwtSecurityToken(header, payloadd);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }

    }
}

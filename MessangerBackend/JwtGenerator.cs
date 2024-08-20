using MessangerBackend.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MessangerBackend
{
    //public static class JwtGenerator
    //{
    //    public static string GenerateJwt(User user, string token, DateTime expiryDate) {
    //        var claims = new List<Claim> {
    //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    //        };
    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));
    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
    //        var jwtToken = new JwtSecurityToken() { 
    //        };
    //    }
    //}
}

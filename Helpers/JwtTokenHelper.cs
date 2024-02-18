using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Vesta.Helpers
{
    public static class JwtTokenHelper
    {
         private static readonly string AccessTokenSecret =  Environment.GetEnvironmentVariable("Vesta:JWT_AccessToken_SecretKey");
         private static readonly string RefreshTokenSecret =  Environment.GetEnvironmentVariable("Vesta:JWT_RefreshToken_SecretKey");
         private static readonly SymmetricSecurityKey AccessTokenSSK =  new(System.Text.Encoding.UTF8.GetBytes(AccessTokenSecret));
         private static readonly SymmetricSecurityKey RefreshTokenSSK =  new(System.Text.Encoding.UTF8.GetBytes(RefreshTokenSecret));
         private static readonly SigningCredentials AccessTokenSigningCredentials =  new(AccessTokenSSK,SecurityAlgorithms.HmacSha512Signature);
         private static readonly SigningCredentials RefreshTokenSigningCredentials =  new(RefreshTokenSSK,SecurityAlgorithms.HmacSha512Signature); 


        public static string CreateAccessToken(List<Claim> claims, int tokenLifeTime_Minutes)
        {
            var expirationDate = DateTime.Now.AddMinutes(tokenLifeTime_Minutes);
            return CreateToken(claims, AccessTokenSigningCredentials, expirationDate);
        }
        public static string CreateRefreshToken(List<Claim> claims, int tokenLifeTime_Hours)
        {
            var expirationDate = DateTime.Now.AddHours(tokenLifeTime_Hours);
            return CreateToken(claims, RefreshTokenSigningCredentials, expirationDate);
        }
        public static string CreateToken(List<Claim> claims, SigningCredentials creds, DateTime expirationDate)
        {
            var token = new JwtSecurityToken(
                claims:claims,
                expires:expirationDate,
                signingCredentials:creds
            );
            var jwToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwToken;
        }
    }
}
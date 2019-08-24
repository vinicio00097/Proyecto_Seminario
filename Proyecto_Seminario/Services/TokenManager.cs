using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Proyecto_Seminario.Services
{
    public class TokenManager
    {
        private static string Secret = "a1hYx1UKfKAYm6Lfdv4c2MJTusmuOclabGXpKflgommW5VCgV14k1abym2yONZD26hxozdZdANv4UyDodBipcxDxcs7kSYFz8r0wsXhEyCeNVFbFBBsWPDopdIuPoG7G";


        public static string GenerateToken(string user,string user_level)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                      new Claim("user_email", user),
                      new Claim("user_level", user_level),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool ValidateToken(string token)
        {
            //string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return false;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return false;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            //username = usernameClaim.Value;
            return true;
        }

        public static async System.Threading.Tasks.Task<bool> ValidateGoogleToken(string token)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);

                if(payload.ExpirationTimeSeconds> DateTimeOffset.Now.ToUnixTimeSeconds())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(InvalidJwtException exc)
            {
                Debug.WriteLine(exc);
                return false;
            }
        }

        public static ClaimsIdentity getClaims(string token)
        {
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            return identity;
        }
    }

}
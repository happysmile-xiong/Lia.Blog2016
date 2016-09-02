using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Infrastructure
{
    public static class LocationClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ClaimsIdentity user)
        {
            var claims = new List<Claim>();
            if(user.Name.ToLower().Equals("alice"))
            {
                claims.Add(CreateClaim(ClaimTypes.PostalCode, "0154545"));
                claims.Add(CreateClaim(ClaimTypes.StateOrProvince,"Beijing"));
            }
            else
            {
                claims.Add(CreateClaim(ClaimTypes.PostalCode, "8987498"));
                claims.Add(CreateClaim(ClaimTypes.StateOrProvince, "Shanghai"));
            }
            return claims;
        }

        private static Claim CreateClaim(string type,string value)
        {
            return new Claim(type, value, ClaimValueTypes.String, "RemoteClaims");
        }
    }
}

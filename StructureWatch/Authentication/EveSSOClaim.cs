using System.Collections.Generic;
using System.Security.Claims;
using wilhe1m.StructureWatch.Models;


//This is based on  https://github.com/dstegelman/ShibExample/
namespace wilhe1m.StructureWatch.Authentication
{
    /// <summary>
    ///     Represents a shibboleth user auth claim
    /// </summary>
    public class EveSSOClaim : IEveSSOClaim
    {
        private const string issuer = "Structure Watch";
        private readonly List<Claim> _userClaims;


        /// <summary>
        ///     Basic onstruictor
        /// </summary>
        public EveSSOClaim()
        {
            _userClaims = new List<Claim>();
        }

        /// <summary>
        ///     Main entry point for establishing a principal.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public ClaimsPrincipal BuildClaimsPrincipal(StructureContext context, string username)
        {
            BuildUserClaim(username);
            SetPermissionClaims(context, username);
            var userIdentity = new ClaimsIdentity(_userClaims, "Passport");
            return new ClaimsPrincipal(userIdentity);
        }

        /// <summary>
        ///     Formal method for adding the generic username claim to the identity.  This is where
        ///     other claims can be added as well.
        /// </summary>
        /// <param name="username"></param>
        private void BuildUserClaim(string username)
        {
            _userClaims.Add(new Claim(ClaimTypes.Name, username, ClaimValueTypes.String)); //, issuer));
        }

        /// <summary>
        ///     Set permission claims.  You can assign a user
        ///     different policy claims based upon their username.
        /// </summary>
        private void SetPermissionClaims(StructureContext context, string username)
        {
            // if we get where we have a db  record.
            _userClaims.Add(new Claim(ClaimTypes.Role, "User", ClaimValueTypes.String)); //, issuer));
        }
    }
}
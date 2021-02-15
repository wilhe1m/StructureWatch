using System.Security.Claims;
using wilhe1m.StructureWatch.Models;


//This is based on  https://github.com/dstegelman/ShibExample/
namespace wilhe1m.StructureWatch.Authentication
{
    /// <summary>
    /// Represents a shibboleth user auth claim
    /// </summary>
    public interface IEveSSOClaim
    {
        /// <summary>
        /// Build you a claim principle
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        ClaimsPrincipal BuildClaimsPrincipal(StructureContext context, string username);
    }
}

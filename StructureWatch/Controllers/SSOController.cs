using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using wilhe1m.StructureWatch.Authentication;
using wilhe1m.StructureWatch.Models;
using wilhe1m.StructureWatch.Services;

namespace wilhe1m.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    ///https://developers.eveonline.com/blog/article/sso-to-authenticated-calls
    public class SSOController : ControllerBase
    {
        [HttpGet]
        [Route("/api/SSO/Login")]
        public void GetSSOSignInURL()
        {
            Response.Redirect(EVESwagger.GetAuthUrl(new List<string> {"esi-characters.read_notifications.v1"},
                "Login"));
        }

        [HttpGet]
        [Route("/api/SSO/Logout")]
        public async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Redirect("/");
        }

        [HttpGet]
        public async Task GetCodeAsync(string code, string state)
        {
            var token = await EVESwagger.GetToken("authorization_code", code);
            var auth_char = await EVESwagger.Verify(token);


            using (var context = new StructureContext())
            {
                //if we have on update it to use the token we just got or bail.
                var fromdb = context.Characters.Where(c => c.CharacterID == auth_char.CharacterID).FirstOrDefault();
                if (fromdb == null)
                {
                    context.Characters.Attach(auth_char);
                }
                else
                {
                    fromdb.ConsumeToken(token);
                    context.Characters.Update(fromdb);
                }

                context.SaveChanges();


                //sign in the user:
                var principal = new EveSSOClaim().BuildClaimsPrincipal(context, auth_char.CharacterName);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.User = principal;
            }

            Response.Redirect("/LoggedIn");
        }
    }
}
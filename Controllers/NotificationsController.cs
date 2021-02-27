using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wilhe1m.StructureWatch.Models;
using wilhe1m.StructureWatch.Services;

namespace wilhe1m.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    ///https://developers.eveonline.com/blog/article/sso-to-authenticated-calls
    public class NotificationsController : ControllerBase
    {
        [HttpGet]
        public async Task GetNotifications()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
                using (var context = new StructureContext())
                {
                    var character = context.Characters.Where(c => c.CharacterName == User.Identity.Name)
                        .FirstOrDefault();
                    if (character != null) await Polling.UpdateOneCharacter(context, character);
                }

            Response.Redirect("/Me/Notifications");
        }

        [HttpGet]
        [Route("All")]
        public async Task FullyUpdateData()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
                using (var context = new StructureContext())
                {
                    await Polling.FullyUpdateData(context);
                }

            Response.Redirect("/Me/Notifications");
        }

        [HttpGet]
        [Route("Hide")]
        public async Task Hide(long id)
        {
            using (var context = new StructureContext())
            {
                var notif = context.Notifications.Find(id);
                if (notif != null)
                {
                    notif.Hidden = true;
                    context.Update(notif);
                    await context.SaveChangesAsync();
                }
            }


            Response.Redirect("/Me/Notifications");
        }
    }
}
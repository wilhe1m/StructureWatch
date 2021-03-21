using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wilhe1m.StructureWatch.Models;

namespace wilhe1m.StructureWatch.Pages.Me
{
    [Authorize]
    public class structureModel : PageModel
    {
        private readonly StructureContext context;

        public structureModel(StructureContext _context)
        {
            context = _context;
        }

        public Structure Structure { get; set; }
        public List<Notification> NotificationsAboutId { get; set; } = new List<Notification>();

        public void OnGet(long id)
        {
            NotificationsAboutId = context.Notifications.Where(n => n.Text.Contains("//" + id + "\""))
                .OrderByDescending(n => n.Timestamp).ToList();
            Structure = context.Structures.Where(s => s.StructureId == id).FirstOrDefault();
            if (Structure == null && NotificationsAboutId.Count > 0)
            {
                //create it if we have seen it before in notificiations.
                Structure = new Structure();
                Structure.Name = Regex.Replace(NotificationsAboutId[0].ParsedData["structureLink"], "<.+?>", "");
                Structure.StructureId = long.Parse(Regex
                    .Match(NotificationsAboutId[0].ParsedData["structureLink"], @"//(\d+)").Groups[1].Value);
                Structure.FirstSeen = NotificationsAboutId[NotificationsAboutId.Count - 1].Timestamp;

                context.Attach(Structure);
                context.SaveChanges();
            }
        }
    }
}
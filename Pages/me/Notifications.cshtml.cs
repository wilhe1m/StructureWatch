using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wilhe1m.StructureWatch.Models;


namespace  wilhe1m.StructureWatch.Pages.Me
{
     [Authorize]
    public class NotificiationsModel : PageModel
    {
        private readonly StructureContext context;

        public NotificiationsModel(StructureContext _context){
            context = _context;
        }

        public List<Notification> Notifications{get;set;} = new List<Notification>();
        public void OnGet(bool showAll = false)
        {
            Notifications = context.Notifications
            .Where(n=> showAll || ( n.Hidden == false && n.Timestamp > DateTime.UtcNow.AddMonths(-1)))
            .OrderByDescending(n => n.Timestamp).ToList();
           
        }
    }
}

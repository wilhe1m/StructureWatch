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
    public class indexModel : PageModel
    {
        private readonly StructureContext context;

        public indexModel(StructureContext _context){
            context = _context;
        }

        public List<Notification> Notifications{get;set;} = new List<Notification>();
        public void OnGet()
        {
            Notifications = context.Notifications.OrderByDescending(n => n.Timestamp).ToList();
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wilhe1m.StructureWatch.Models;
using wilhe1m.StructureWatch.Services;

namespace  wilhe1m.StructureWatch.Pages
{
    public class IndexModel : PageModel
    {
        private readonly StructureContext context;

        public IndexModel(StructureContext _context){
            context = _context;
        }
        public void OnGet(int id)
        {
         
        }
    }
}

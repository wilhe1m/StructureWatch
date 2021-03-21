using Microsoft.AspNetCore.Mvc.RazorPages;
using wilhe1m.StructureWatch.Models;

namespace wilhe1m.StructureWatch.Pages
{
    public class IndexModel : PageModel
    {
        private readonly StructureContext context;

        public IndexModel(StructureContext _context)
        {
            context = _context;
        }

        public void OnGet(int id)
        {
        }
    }
}
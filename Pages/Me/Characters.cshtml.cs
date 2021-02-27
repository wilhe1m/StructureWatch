using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wilhe1m.StructureWatch.Models;

namespace wilhe1m.StructureWatch.Pages.Me
{
    [Authorize]
    public class CharacterModel : PageModel
    {
        private readonly StructureContext context;

        public CharacterModel(StructureContext _context)
        {
            context = _context;
        }

        public List<Character> Characters { get; set; } = new List<Character>();

        public void OnGet()
        {
            Characters = context.Characters.ToList();
        }
    }
}
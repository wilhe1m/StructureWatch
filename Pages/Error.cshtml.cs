using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wilhe1m.StructureWatch.Pages
{
    public class ErrorModel : PageModel
    {
        public Exception exception { get; set; }

        public void OnGet()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            exception = context.Error;
        }
    }
}